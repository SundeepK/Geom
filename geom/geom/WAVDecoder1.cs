using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System.Windows;

using System.Diagnostics;


//using NAudio;
//using NAudio.Wave;

namespace geom
{
    public class WAVDecoder 
    {

        DynamicSoundEffectInstance dynamicSound;
   
        private  float MAX_VALUE = 1.0f / short.MaxValue;

        /** Aquired from http://code.google.com/p/audio-analysis/
         * the input stream we read from **/
        private EndianDataInputStream enDataStream;
	
	/** number of channels **/
	    public  int channels;
        public BinaryReader reader;
	/** sample rate in Herz**/
	    private  float sampleRate;

        public WAVDecoder(string fileName) 
        {

            //texture = GameServices.GetService<ContentManager>().Load<Texture2D>(@"player copy");

            Stream waveFileStream = new FileStream(@fileName, FileMode.Open, FileAccess.Read);
            //Stream wave = waveFileStream.
            //Stream waveFileStream = TitleContainer.OpenStream(fileName);

            //GameServices.GetService<ContentManager>().Load < SoundEffect >
           //reader = new BinaryReader(waveFileStream);

            //int chunkID = reader.ReadInt32();
            //int fileSize = reader.ReadInt32();
            //int riffType = reader.ReadInt32();
            //int fmtID = reader.ReadInt32();
            //int fmtSize = reader.ReadInt32();
            //int fmtCode = reader.ReadInt16();
            // channels = reader.ReadInt16();
            // sampleRate = reader.ReadInt32();
            //int fmtAvgBPS = reader.ReadInt32();
            //int fmtBlockAlign = reader.ReadInt16();
            //int bitDepth = reader.ReadInt16();

            ////if (fmtSize == 18)
            ////{
            ////    // Read any extra values
            ////    int fmtExtraSize = reader.ReadInt16();
            ////    reader.ReadBytes(fmtExtraSize);
            ////}

            //int dataID = reader.ReadInt32();

            //int dataSize = reader.ReadInt32();
           
            //byte[] bytes = BitConverter.GetBytes(channels);
            //string ss = System.Text.Encoding.UTF8.GetString(bytes);

            //Stream waveFileStream = TitleContainer.OpenStream(fileName);
            //BinaryReader reader = new BinaryReader(waveFileStream );

           enDataStream = new EndianDataInputStream(waveFileStream);

           if (!enDataStream.read4ByteString().Equals("RIFF"))
               throw new ArgumentException("not a wav");

           enDataStream.readIntLittleEndian();

           if (!enDataStream.read4ByteString().Equals("WAVE"))
               throw new ArgumentException("expected WAVE tag");

           if (!enDataStream.read4ByteString().Equals("fmt "))
               throw new ArgumentException("expected fmt tag");

           if (enDataStream.readIntLittleEndian() != 16)
               //throw new ArgumentException("expected wave chunk size to be 16");

           if (enDataStream.readShortLittleEndian() != 1)
               throw new ArgumentException("expected format to be 1");

           channels = enDataStream.readShortLittleEndian();
           sampleRate = enDataStream.readIntLittleEndian();

           if (sampleRate != 44100)
               throw new ArgumentException("Not 44100 sampling rate");

           enDataStream.readIntLittleEndian();
           enDataStream.readShortLittleEndian();
           int fmt = enDataStream.readShortLittleEndian();

           if (fmt != 16)
               throw new ArgumentException("Only 16-bit signed format supported");

           if (!enDataStream.read4ByteString().Equals("data"))
               //throw new ArgumentException("expected data tag");

          enDataStream.readIntLittleEndian();
          //Debug.WriteLine(g);

        }

        public int readSamples(float[] samples)
        {
            int readSamples = 0;
            for (int i = 0; i < samples.Length; i++)
            {
                float sample = 0;
                try
                {
                    for (int j = 0; j < 2; j++)
                    {
                        int shortValue = enDataStream.readShortLittleEndian();
                        sample += (shortValue * MAX_VALUE);
                    }
                    sample /= channels;
                    samples[i] = sample;
                    readSamples++;
                }
                catch (Exception ex)
                {
                    break;
                }
            }

            return readSamples;
            //		return samples.length;
        }

    }
}
