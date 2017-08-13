using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ImageCrawler.Util
{
    public class StreamConnector
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sourceStream"></param>
        /// <param name="targetStream"></param> 
        public static void connect(Stream sourceStream, Stream targetStream)
        {
            int bufferSize = 4096;
            StreamConnector.connect(sourceStream, targetStream, bufferSize,true);
        }

        public static void connect(Stream sourceStream, Stream targetStream, int bufferSize, Boolean flushWhenFinished)
        {
            StreamConnector.connect(sourceStream, targetStream, bufferSize, flushWhenFinished, null);
        }

        public delegate void StreamReadEvent(long currentBytesRead, out Boolean doAbort);
        public static void connect(Stream sourceStream, Stream targetStream, int bufferSize, Boolean flushWhenFinished,StreamReadEvent streamReadEvent )
        {
            //
            Byte[] buffer = new Byte[bufferSize];
            int iRead = 1;

            //
            if (streamReadEvent != null)
            {
                long currentBytesRead = 0;
                Boolean doAbort = false;
                while (iRead > 0 && !doAbort)
                {
                    //
                    iRead = sourceStream.Read(buffer, 0, bufferSize);
                    targetStream.Write(buffer, 0, iRead);

                    //
                    currentBytesRead += iRead;
                    
                    streamReadEvent(currentBytesRead,out doAbort);
                }
            }
            else
            {
                while (iRead > 0)
                {
                    iRead = sourceStream.Read(buffer, 0, bufferSize);
                    targetStream.Write(buffer, 0, iRead);
                }
            }

            //
            if (flushWhenFinished)
            {
                targetStream.Flush();
            }
        }


        /// <summary>
        /// Defines a stream, which has an event available to report the currently read number of bytes.
        /// </summary>
        public class ProgressReportStream : Stream
        {
            private Stream sourceStream=null;
            private long readBytesCounter = 0;
            private long writtenBytesCounter = 0;

            public event EventHandler<EventArgs> progressUpdateEvent;

            public ProgressReportStream(Stream sourceStream)
            {
                this.sourceStream = sourceStream;
            }

            public override bool CanRead
            {
                get { return this.sourceStream != null && this.sourceStream.CanRead; }
            }

            public override bool CanSeek
            {
                get { return this.sourceStream != null && this.sourceStream.CanSeek; }
            }

            public override bool CanWrite
            {
                get { return this.sourceStream != null && this.sourceStream.CanWrite; }
            }

            public override void Flush()
            {
                if (this.sourceStream != null)
                {
                    this.sourceStream.Flush();
                }
            }

            public override long Length
            {
                get { return this.sourceStream == null ? 0 : this.sourceStream.Length; }
            }

            public override long Position
            {
                get
                {
                    return this.sourceStream == null ? -1 : this.sourceStream.Position; 
                }
                set
                {
                    if (this.sourceStream != null)
                    {
                        this.sourceStream.Position = value;
                    }
                }
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                if (this.sourceStream != null)
                {
                    //
                    int readCount = this.sourceStream.Read(buffer, offset, count);

                    //
                    this.readBytesCounter += readCount;

                    this.triggerProgressUpdateEvent();

                    //
                    return readCount;
                }
                else
                {
                    return 0;
                }
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                if (this.sourceStream != null)
                {
                    return this.sourceStream.Seek(offset, origin);
                }
                else
                {
                    return -1;
                }
            }

            public override void SetLength(long value)
            {
                if (this.sourceStream != null)
                {
                    this.sourceStream.SetLength(value);
                }
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                if (this.sourceStream != null)
                {
                    //
                    this.sourceStream.Write(buffer, offset, count);

                    //
                    this.writtenBytesCounter += count;

                    this.triggerProgressUpdateEvent();
                }
            }

            private void triggerProgressUpdateEvent()
            {
                if (this.progressUpdateEvent != null)
                {
                    this.progressUpdateEvent(this, new EventArgs(this.readBytesCounter, this.writtenBytesCounter));
                }
            }

            public class EventArgs : System.EventArgs
            {
                public long readBytesCounter = 0;
                public long writtenBytesCounter = 0;

                public EventArgs(long readBytesCounter, long writtenBytesCounter)
                {
                    this.readBytesCounter = readBytesCounter;
                    this.writtenBytesCounter = writtenBytesCounter;
                }
            }
        }
    }
}
