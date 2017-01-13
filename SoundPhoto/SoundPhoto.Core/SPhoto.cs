using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace SoundPhoto.Core
{
    public class SPhoto
    {
        private byte[] _buffer;
        private byte[] _image;
        private byte[] _audio;
        private string _path;

        public SPhoto(string path)
        {
            _path = path;
        }

        public void CreateBasicSPhoto(string fileNamePhoto, string fileNameAudio)
        {
            _image = File.ReadAllBytes(fileNamePhoto);
            _audio = File.ReadAllBytes(fileNameAudio);
            _buffer = new byte[_image.Length + _audio.Length + Constants.HeaderLength + Constants.MagicPatternLength];
            for (int i = 0; i < Constants.HeaderLength; i++)
            {
                _buffer[i] = Convert.ToByte(Constants.HeaderId[i]);
            }
            MemoryStream writer = new MemoryStream(_buffer);
            writer.Seek(Constants.HeaderLength, 0);
            writer.Write(_image, 0, _image.Length);
            writer.Seek(Constants.HeaderLength + _image.Length, 0);
            writer.Write(Encoding.UTF8.GetBytes(Constants.MagicPattern), 0, Constants.MagicPatternLength);
            writer.Seek(Constants.HeaderLength + _image.Length + Constants.MagicPatternLength, 0);
            writer.Write(_audio, 0, _audio.Length);
            FileStream fs = new FileStream(_path, FileMode.Create);
            writer.WriteTo(fs);
            writer.Close();
            fs.Close();
        }

        public void OpenExistingSPhoto()
        {
            _buffer = File.ReadAllBytes(_path);
            MemoryStream ms = new MemoryStream(_buffer);
            byte[] header = new byte[Constants.HeaderLength];
            ms.Read(header, 0, Constants.HeaderLength);
            if (Encoding.UTF8.GetString(header) != "BHAU")
            {
                throw new Exception("Invalid file");
            }
            ms.Seek(Constants.HeaderLength, 0);
            int loc = Locate(_buffer, Encoding.UTF8.GetBytes(Constants.MagicPattern));
            if (loc == -1)
                throw new Exception("Corrupt File");
            _image = new byte[loc - Constants.HeaderLength];
            _audio = new byte[_buffer.Length - (loc + Constants.MagicPatternLength)];
            ms.Read(_image, 0, loc - Constants.HeaderLength);
            ms.Seek(loc + Constants.MagicPatternLength, 0);
            ms.Read(_audio, 0, _buffer.Length - (loc + Constants.MagicPatternLength));
        }

        public Image GetImage()
        {
            using (var im = new MemoryStream(_image))
            {
                return Image.FromStream(im);
            }
        }

        public byte[] GetAudio()
        {
            return _audio;
        }



        // Bad Code
        public int Locate(byte[] self, byte[] candidate)
        {
            for (int i = 0; i < self.Length; i++)
            {
                if (IsMatch(self, i, candidate))
                    return i;
                else
                    continue;
            }
            return -1;
        }

        static bool IsMatch(byte[] array, int position, byte[] candidate)
        {
            if (candidate.Length > (array.Length - position))
                return false;
            for (int i = 0; i < candidate.Length; i++)
                if (array[position + i] != candidate[i])
                    return false;
            return true;
        }
    }
}
