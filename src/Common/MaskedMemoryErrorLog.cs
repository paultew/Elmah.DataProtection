#region License, Terms and Author(s)
//
// ELMAH - Error Logging Modules and Handlers for ASP.NET
// Copyright (c) 2004-9 Atif Aziz. All rights reserved.
//
//  Author(s):
//
//      James Driscoll, mailto:jamesdriscoll@btinternet.com
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

namespace Elmah.Masking
{
    using System;
    using System.Collections;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Threading;

    public sealed class MaskedMemoryErrorLog : ErrorLog
    {
        private class EntryCollection : NameObjectCollectionBase
		{
			private readonly int _size;

			public ErrorLogEntry this[int index]
			{
				get
				{
					return (ErrorLogEntry)base.BaseGet(index);
				}
			}

			public ErrorLogEntry this[Guid id]
			{
				get
				{
					return (ErrorLogEntry)base.BaseGet(id.ToString());
				}
			}

			public ErrorLogEntry this[string id]
			{
				get
				{
					return this[new Guid(id)];
				}
			}

			public EntryCollection(int size) : base(size)
			{
				this._size = size;
			}

			public void Add(ErrorLogEntry entry)
			{
				if (this.Count == this._size)
				{
					base.BaseRemoveAt(0);
				}
				base.BaseAdd(entry.Id, entry);
			}
		}

        private MaskedValuesConfigurationSection _obfuscationConfiguration;

		private static MaskedMemoryErrorLog.EntryCollection _entries;
		private static readonly ReaderWriterLock _lock = new ReaderWriterLock();

		private readonly int _size;

		public static readonly int MaximumSize = 500;
		public static readonly int DefaultSize = 15;

		public override string Name
		{
			get
			{
				return "In-Memory Error Log";
			}
		}

		public MaskedMemoryErrorLog() : this(MemoryErrorLog.DefaultSize)
		{
		}

		public MaskedMemoryErrorLog(int size)
		{
			if (size < 0 || size > MemoryErrorLog.MaximumSize)
			{
				throw new ArgumentOutOfRangeException("size", size, string.Format("Size must be between 0 and {0}.", MemoryErrorLog.MaximumSize));
			}
			this._size = size;

            this._obfuscationConfiguration = (MaskedValuesConfigurationSection)ElmahConfiguration.GetSubsection(MaskedValuesConfigurationSection.SectionName);
            if (this._obfuscationConfiguration == null)
            {
                this._obfuscationConfiguration = new MaskedValuesConfigurationSection();
            }
		}

        public MaskedMemoryErrorLog(IDictionary config)
		{
			if (config == null)
			{
				this._size = MemoryErrorLog.DefaultSize;
				return;
			}
			string text = config["size"] != null ? (string)config["size"] : string.Empty;
			if (text.Length == 0)
			{
				this._size = MemoryErrorLog.DefaultSize;
				return;
			}
			this._size = Convert.ToInt32(text, CultureInfo.InvariantCulture);
			this._size = Math.Max(0, Math.Min(MemoryErrorLog.MaximumSize, this._size));

            this._obfuscationConfiguration = (MaskedValuesConfigurationSection)ElmahConfiguration.GetSubsection(MaskedValuesConfigurationSection.SectionName);
            if (this._obfuscationConfiguration == null)
            {
                this._obfuscationConfiguration = new MaskedValuesConfigurationSection();
            }
		}

        public override ErrorLogEntry GetError(string id)
        {
            MaskedMemoryErrorLog._lock.AcquireReaderLock(-1);
            ErrorLogEntry errorLogEntry;
            try
            {
                if (MaskedMemoryErrorLog._entries == null)
                {
                    ErrorLogEntry result = null;
                    return result;
                }
                errorLogEntry = MaskedMemoryErrorLog._entries[id];
            }
            finally
            {
                MaskedMemoryErrorLog._lock.ReleaseReaderLock();
            }
            if (errorLogEntry == null)
            {
                return null;
            }
            Error error = (Error)((ICloneable)errorLogEntry.Error).Clone();
            return new ErrorLogEntry(this, errorLogEntry.Id, error);
        }

        public override int GetErrors(int pageIndex, int pageSize, IList errorEntryList)
        {
            if (pageIndex < 0)
            {
                throw new ArgumentOutOfRangeException("pageIndex", pageIndex, null);
            }
            if (pageSize < 0)
            {
                throw new ArgumentOutOfRangeException("pageSize", pageSize, null);
            }
            ErrorLogEntry[] array = null;
            MaskedMemoryErrorLog._lock.AcquireReaderLock(-1);
            int count;
            try
            {
                if (MaskedMemoryErrorLog._entries == null)
                {
                    return 0;
                }
                count = MaskedMemoryErrorLog._entries.Count;
                int num = pageIndex * pageSize;
                int num2 = Math.Min(num + pageSize, count);
                int num3 = Math.Max(0, num2 - num);
                if (num3 > 0)
                {
                    array = new ErrorLogEntry[num3];
                    int i = num2;
                    int num4 = 0;
                    while (i > num)
                    {
                        array[num4++] = MaskedMemoryErrorLog._entries[--i];
                    }
                }
            }
            finally
            {
                MaskedMemoryErrorLog._lock.ReleaseReaderLock();
            }
            if (errorEntryList != null && array != null)
            {
                ErrorLogEntry[] array2 = array;
                for (int j = 0; j < array2.Length; j++)
                {
                    ErrorLogEntry errorLogEntry = array2[j];
                    Error error = (Error)((ICloneable)errorLogEntry.Error).Clone();
                    errorEntryList.Add(new ErrorLogEntry(this, errorLogEntry.Id, error));
                }
            }
            return count;
        }

        public override string Log(Error error)
        {
            if (error == null)
            {
                throw new ArgumentNullException("error");
            }
            error = (Error)((ICloneable)error).Clone();

            ErrorHelper.Obscure(error, this._obfuscationConfiguration);

            error.ApplicationName = base.ApplicationName;
            Guid guid = Guid.NewGuid();
            ErrorLogEntry entry = new ErrorLogEntry(this, guid.ToString(), error);
            MaskedMemoryErrorLog._lock.AcquireWriterLock(-1);
            try
            {
                if (MaskedMemoryErrorLog._entries == null)
                {
                    MaskedMemoryErrorLog._entries = new MaskedMemoryErrorLog.EntryCollection(this._size);
                }
                MaskedMemoryErrorLog._entries.Add(entry);
            }
            finally
            {
                MaskedMemoryErrorLog._lock.ReleaseWriterLock();
            }
            return guid.ToString();
        }
    }
}
