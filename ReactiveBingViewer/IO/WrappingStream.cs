using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactiveBingViewer.IO
{
    /// <summary>
    /// ストリームのラッパクラス
    /// </summary>
    /// <remarks>
    /// Dispose 時に、ベースストリームの参照を外します
    /// </remarks>
    public class WrappingStream : Stream
    {
    
        public WrappingStream(Stream streamBase)
        {
            
            if (streamBase == null)
            {
                throw new ArgumentNullException("streamBase");
            }
            m_streamBase = streamBase;
        }

        public override bool CanRead
        {
            get { return m_streamBase == null ? false : m_streamBase.CanRead; }
        }

        
        public override bool CanSeek
        {
            get { return m_streamBase == null ? false : m_streamBase.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return m_streamBase == null ? false : m_streamBase.CanWrite; }
        }

        public override long Length
        {
            get { ThrowIfDisposed(); return m_streamBase.Length; }
        }

        
        public override long Position
        {
            get { ThrowIfDisposed(); return m_streamBase.Position; }
            set { ThrowIfDisposed(); m_streamBase.Position = value; }
        }

        
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            ThrowIfDisposed();
            return m_streamBase.BeginRead(buffer, offset, count, callback, state);
        }

        
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            ThrowIfDisposed();
            return m_streamBase.BeginWrite(buffer, offset, count, callback, state);
        }
        

        public override int EndRead(IAsyncResult asyncResult)
        {
            ThrowIfDisposed();
            return m_streamBase.EndRead(asyncResult);
        }

        public override void EndWrite(IAsyncResult asyncResult)
        {
            ThrowIfDisposed();
            m_streamBase.EndWrite(asyncResult);
        }

        public override void Flush()
        {
            ThrowIfDisposed();
            m_streamBase.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed();
            return m_streamBase.Read(buffer, offset, count);
        }

        public override int ReadByte()
        {
            ThrowIfDisposed();
            return m_streamBase.ReadByte();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            ThrowIfDisposed();
            return m_streamBase.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            ThrowIfDisposed();
            m_streamBase.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed();
            m_streamBase.Write(buffer, offset, count);
        }

        public override void WriteByte(byte value)
        {
            ThrowIfDisposed();
            m_streamBase.WriteByte(value);
        }

        public override Task WriteAsync(byte[] buffer, int offset, int count, System.Threading.CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            return m_streamBase.WriteAsync(buffer, offset, count, cancellationToken);
        }
        public new Task WriteAsync(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed();
            return m_streamBase.WriteAsync(buffer, offset, count);
        }

        public override Task<int> ReadAsync(byte[] buffer, int offset, int count, System.Threading.CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            return m_streamBase.ReadAsync(buffer, offset, count, cancellationToken);
        }

        public new Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed();
            return m_streamBase.ReadAsync(buffer, offset, count);
        }

        public override Task CopyToAsync(Stream destination, int bufferSize, System.Threading.CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            return m_streamBase.CopyToAsync(destination, bufferSize, cancellationToken);
        }
        public new Task CopyToAsync(Stream destination, int bufferSize)
        {
            ThrowIfDisposed();
            return m_streamBase.CopyToAsync(destination, bufferSize);
        }

        public override Task FlushAsync(System.Threading.CancellationToken cancellationToken)
        {
            ThrowIfDisposed();
            return m_streamBase.FlushAsync(cancellationToken);
        }

        public new Task FlushAsync()
        {
            ThrowIfDisposed();
            return m_streamBase.FlushAsync();
        }

        protected Stream WrappedStream
        {
            get { return m_streamBase; }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_streamBase = null;
            }
            base.Dispose(disposing);
        }

        private void ThrowIfDisposed()
        {
            if (m_streamBase == null)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        Stream m_streamBase;
    }

}
