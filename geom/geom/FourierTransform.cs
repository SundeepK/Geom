﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
namespace geom
{
    public abstract class FourierTransform
    {
        /** aquired from http://code.google.com/p/audio-analysis/
         * A constant indicating no window should be used on sample buffers. */
        public static int NONE = 0;
        /** A constant indicating a Hamming window should be used on sample buffers. */
        public const  int HAMMING = 1;
        protected static int LINAVG = 2;
        protected static int LOGAVG = 3;
        protected static int NOAVG = 4;
        protected static float TWO_PI = (float)(2 * Math.PI);
        protected int timeSize;
        protected int sampleRate;
        protected float bandWidth;
        protected int whichWindow;
        protected float[] real;
        protected float[] imag;
        protected float[] spectrum;
        protected float[] averages;
        protected int whichAverage;
        protected int octaves;
        protected int avgPerOctave;

       public FourierTransform(int ts, float sr)
        {
            timeSize = ts;
            sampleRate = (int)sr;
            bandWidth = (2f / timeSize) * ((float)sampleRate / 2f);
            noAverages();
            allocateArrays();
            whichWindow = NONE;
        }

        protected abstract void allocateArrays();

        protected void setComplex(float[] r, float[] i)
        {
            if (real.Length != r.Length && imag.Length != i.Length)
            {
                throw new ArgumentException("This won't work");
            }
            else
            {
                Array.Copy(r, 0, real, 0, r.Length);
                Array.Copy(i, 0, imag, 0, i.Length);
            }
        }

        protected void fillSpectrum()
        {
            for (int i = 0; i < spectrum.Length; i++)
            {
                spectrum[i] = (float)Math.Sqrt(real[i] * real[i] + imag[i] * imag[i]);
            }

            if (whichAverage == LINAVG)
            {
                int avgWidth = (int)spectrum.Length / averages.Length;
                for (int i = 0; i < averages.Length; i++)
                {
                    float avg = 0;
                    int j;
                    for (j = 0; j < avgWidth; j++)
                    {
                        int offset = j + i * avgWidth;
                        if (offset < spectrum.Length)
                        {
                            avg += spectrum[offset];
                        }
                        else
                        {
                            break;
                        }
                    }
                    avg /= j + 1;
                    averages[i] = avg;
                }
            }
            else if (whichAverage == LOGAVG)
            {
                for (int i = 0; i < octaves; i++)
                {
                    float lowFreq, hiFreq, freqStep;
                    if (i == 0)
                    {
                        lowFreq = 0;
                    }
                    else
                    {
                        lowFreq = (sampleRate / 2) / (float)Math.Pow(2, octaves - i);
                    }
                    hiFreq = (sampleRate / 2) / (float)Math.Pow(2, octaves - i - 1);
                    freqStep = (hiFreq - lowFreq) / avgPerOctave;
                    float f = lowFreq;
                    for (int j = 0; j < avgPerOctave; j++)
                    {
                        int offset = j + i * avgPerOctave;
                        averages[offset] = calcAvg(f, f + freqStep);
                        f += freqStep;
                    }
                }
            }
        }

        public void noAverages()
        {
            averages = new float[0];
            whichAverage = NOAVG;
        }

        public void linAverages(int numAvg)
        {
            if (numAvg > spectrum.Length / 2)
            {
                throw new ArgumentException("The number of averages for this transform can be at most " + spectrum.Length / 2 + ".");
            }
            else
            {
                averages = new float[numAvg];
            }
            whichAverage = LINAVG;
        }

        public void logAverages(int minBandwidth, int bandsPerOctave)
        {
            float nyq = (float)sampleRate / 2f;
            octaves = 1;
            while ((nyq /= 2) > minBandwidth)
            {
                octaves++;
            }
            avgPerOctave = bandsPerOctave;
            averages = new float[octaves * bandsPerOctave];
            whichAverage = LOGAVG;
        }

        public void window(int which)
        {
            if (which < 0 || which > 1)
            {
                throw new ArgumentException("Invalid window type.");
            }
            else
            {
                whichWindow = which;
            }
        }

        protected void doWindow(float[] samples)
        {
            switch (whichWindow)
            {
                case HAMMING:
                    hamming(samples);
                    break;
            }
        }

        protected void hamming(float[] samples)
        {
            for (int i = 0; i < samples.Length; i++)
            {
                samples[i] *= (0.54f - 0.46f * (float)Math.Cos((TWO_PI * i) / (samples.Length - 1)));
            }
        }

        public int gettimeSize()
        {
            return timeSize;
        }


        public int specSize()
        {
            return spectrum.Length;
        }

        public float getBand(int i)
        {
            if (i < 0) i = 0;
            if (i > spectrum.Length - 1) i = spectrum.Length - 1;
            return spectrum[i];
        }

        public float getBandWidth()
        {
            return bandWidth;
        }

        public abstract void setBand(int i, float a);

        public abstract void scaleBand(int i, float s);

        public int freqToIndex(float freq)
        {
            // special case: freq is lower than the bandwidth of spectrum[0]
            if (freq < getBandWidth() / 2) return 0;
            // special case: freq is within the bandwidth of spectrum[spectrum.length - 1]
            if (freq > sampleRate / 2 - getBandWidth() / 2) return spectrum.Length - 1;
            // all other cases
            float fraction = freq / (float)sampleRate;
            int i = (int)Math.Round(timeSize * fraction);
            Debug.WriteLine("fef" + i);
            return i;
        }

        public float indexToFreq(int i)
        {
            float bw = getBandWidth();
            // special case: the width of the first bin is half that of the others.
            //               so the center frequency is a quarter of the way.
            if (i == 0) return bw * 0.25f;
            // special case: the width of the last bin is half that of the others.
            if (i == spectrum.Length - 1)
            {
                float lastBinBeginFreq = (sampleRate / 2) - (bw / 2);
                float binHalfWidth = bw * 0.25f;
                return lastBinBeginFreq + binHalfWidth;
            }
            // the center frequency of the ith band is simply i*bw
            // because the first band is half the width of all others.
            // treating it as if it wasn't offsets us to the middle 
            // of the band.
            return i * bw;
        }

        public float getAverageCenterFrequency(int i)
        {
            if (whichAverage == LINAVG)
            {
                // an average represents a certain number of bands in the spectrum
                int avgWidth = (int)(spectrum.Length / averages.Length);
                // the "center" bin of the average, this is fudgy.
                int centerBinIndex = i * avgWidth + avgWidth / 2;
                return indexToFreq(centerBinIndex);

            }
            else if (whichAverage == LOGAVG)
            {
                // which "octave" is this index in?
                int octave = i / avgPerOctave;
                // which band within that octave is this?
                int offset = i % avgPerOctave;
                float lowFreq, hiFreq, freqStep;
                // figure out the low frequency for this octave
                if (octave == 0)
                {
                    lowFreq = 0;
                }
                else
                {
                    lowFreq = (sampleRate / 2) / (float)Math.Pow(2, octaves - octave);
                }
                // and the high frequency for this octave
                hiFreq = (sampleRate / 2) / (float)Math.Pow(2, octaves - octave - 1);
                // each average band within the octave will be this big
                freqStep = (hiFreq - lowFreq) / avgPerOctave;
                // figure out the low frequency of the band we care about
                float f = lowFreq + offset * freqStep;
                // the center of the band will be the low plus half the width
                return f + freqStep / 2;
            }

            return 0;
        }


        public float getFreq(float freq)
        {
            return getBand(freqToIndex(freq));
        }


        public void setFreq(float freq, float a)
        {
            setBand(freqToIndex(freq), a);
        }

        public void scaleFreq(float freq, float s)
        {
            scaleBand(freqToIndex(freq), s);
        }

        public int avgSize()
        {
            return averages.Length;
        }

        public float getAvg(int i)
        {
            float ret;
            if (averages.Length > 0)
                ret = averages[i];
            else
                ret = 0;
            return ret;
        }

        public float calcAvg(float lowFreq, float hiFreq)
        {
            int lowBound = freqToIndex(lowFreq);
            int hiBound = freqToIndex(hiFreq);
            float avg = 0;
            for (int i = lowBound; i <= hiBound; i++)
            {
                avg += spectrum[i];
            }
            avg /= (hiBound - lowBound + 1);
            return avg;
        }

        public abstract void forward(float[] buffer);


        public void forward(float[] buffer, int startAt)
        {
            if (buffer.Length - startAt < timeSize)
            {
                throw new ArgumentException("FourierTransform.forward: not enough samples in the buffer between " + startAt + " and " + buffer.Length + " to perform a transform.");
            }

            // copy the section of samples we want to analyze
            float[] section = new float[timeSize];
            Array.Copy(buffer, startAt, section, 0, section.Length);
            forward(section);
        }

        public abstract void inverse(float[] buffer);

        public void inverse(float[] freqReal, float[] freqImag, float[] buffer)
        {
            setComplex(freqReal, freqImag);
            inverse(buffer);
        }

        public float[] getSpectrum()
        {
            return spectrum;
        }

        /**
         * @return the real part of the last FourierTransform.forward() call.
         */
        public float[] getRealPart()
        {
            return real;
        }

        /**
         * @return the imaginary part of the last FourierTransform.forward() call.
         */
        public float[] getImaginaryPart()
        {
            return imag;
        }


    }
}
