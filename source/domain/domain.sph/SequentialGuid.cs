using System;
using System.Collections.Generic;
using System.Linq;

namespace Bespoke.Sph.Domain
{

    [Serializable]
    public struct SequentialGuid : IComparable<SequentialGuid>, IComparable<Guid>, IComparable
    {
        private const int NumberOfSequenceBytes = 6;
        private const int PermutationsOfAByte = 256;
        private static readonly long m_maximumPermutations =
        (long)Math.Pow(PermutationsOfAByte, NumberOfSequenceBytes);
        private static long m_lastSequence;


        private static readonly DateTime m_sequencePeriodStart =
        new DateTime(2011, 11, 15, 0, 0, 0, DateTimeKind.Utc); // Start = 000000

        private static readonly DateTime m_sequencePeriodeEnd =
    new DateTime(2100, 1, 1, 0, 0, 0, DateTimeKind.Utc);   // End   = FFFFFF

        private readonly Guid m_guidValue;

        public SequentialGuid(Guid guidValue)
        {
            m_guidValue = guidValue;
        }

        public SequentialGuid(string guidValue)
        : this(new Guid(guidValue))
        {
        }

        [System.Security.SecuritySafeCritical]
        public static SequentialGuid NewSequentialGuid()
        {
            // You might want to inject DateTime.Now in production code
            return new SequentialGuid(GetGuidValue(DateTime.Now));
        }

        public static TimeSpan TimePerSequence
        {
            get
            {
                var ticksPerSequence = TotalPeriod.Ticks / m_maximumPermutations;
                var result = new TimeSpan(ticksPerSequence);
                return result;
            }
        }

        public static TimeSpan TotalPeriod
        {
            get
            {
                var result = m_sequencePeriodeEnd - m_sequencePeriodStart;
                return result;
            }
        }

        #region FromDateTimeToGuid

        // Internal for testing
        internal static Guid GetGuidValue(DateTime now)
        {
            if (now < m_sequencePeriodStart || now >= m_sequencePeriodeEnd)
            {
                return Guid.NewGuid(); // Outside the range, use regular Guid
            }

            var sequence = GetCurrentSequence(now);
            return GetGuid(sequence);
        }

        private static long GetCurrentSequence(DateTime now)
        {
            var ticksUntilNow = now.Ticks - m_sequencePeriodStart.Ticks;
            var factor = (decimal)ticksUntilNow / TotalPeriod.Ticks;
            var resultDecimal = factor * m_maximumPermutations;
            var resultLong = (long)resultDecimal;
            return resultLong;
        }

        private static readonly object m_synchronizationObject = new object();
        private static Guid GetGuid(long sequence)
        {
            lock (m_synchronizationObject)
            {
                if (sequence <= m_lastSequence)
                {
                    // Prevent double sequence on same server
                    sequence = m_lastSequence + 1;
                }
                m_lastSequence = sequence;
            }

            var sequenceBytes = GetSequenceBytes(sequence);
            var guidBytes = GetGuidBytes();
            var totalBytes = guidBytes.Concat(sequenceBytes).ToArray();
            var result = new Guid(totalBytes);
            return result;
        }

        private static IEnumerable<byte> GetSequenceBytes(long sequence)
        {
            var sequenceBytes = BitConverter.GetBytes(sequence);
            var sequenceBytesLongEnough = sequenceBytes.Concat(new byte[NumberOfSequenceBytes]);
            var result = sequenceBytesLongEnough.Take(NumberOfSequenceBytes).Reverse();
            return result;
        }

        private static IEnumerable<byte> GetGuidBytes()
        {
            return Guid.NewGuid().ToByteArray().Take(10);
        }

        #endregion

        #region FromGuidToDateTime

        public DateTime CreatedDateTime => GetCreatedDateTime(m_guidValue);

        internal static DateTime GetCreatedDateTime(Guid value)
        {
            var sequenceBytes = GetSequenceLongBytes(value).ToArray();
            var sequenceLong = BitConverter.ToInt64(sequenceBytes, 0);
            var sequenceDecimal = (decimal)sequenceLong;
            var factor = sequenceDecimal / m_maximumPermutations;
            var ticksUntilNow = factor * TotalPeriod.Ticks;
            var nowTicksDecimal = ticksUntilNow + m_sequencePeriodStart.Ticks;
            var nowTicks = (long)nowTicksDecimal;
            var result = new DateTime(nowTicks);
            return result;
        }

        private static IEnumerable<byte> GetSequenceLongBytes(Guid value)
        {
            const int NUMBER_OF_BYTES_OF_LONG = 8;
            var sequenceBytes = value.ToByteArray().Skip(10).Reverse().ToArray();
            var additionalBytesCount = NUMBER_OF_BYTES_OF_LONG - sequenceBytes.Length;
            return sequenceBytes.Concat(new byte[additionalBytesCount]);
        }

        #endregion

        #region Relational Operators

        public static bool operator <(SequentialGuid value1, SequentialGuid value2)
        {
            return value1.CompareTo(value2) < 0;
        }

        public static bool operator >(SequentialGuid value1, SequentialGuid value2)
        {
            return value1.CompareTo(value2) > 0;
        }

        public static bool operator <(Guid value1, SequentialGuid value2)
        {
            return value1.CompareTo(value2) < 0;
        }

        public static bool operator >(Guid value1, SequentialGuid value2)
        {
            return value1.CompareTo(value2) > 0;
        }

        public static bool operator <(SequentialGuid value1, Guid value2)
        {
            return value1.CompareTo(value2) < 0;
        }

        public static bool operator >(SequentialGuid value1, Guid value2)
        {
            return value1.CompareTo(value2) > 0;
        }

        public static bool operator <=(SequentialGuid value1, SequentialGuid value2)
        {
            return value1.CompareTo(value2) <= 0;
        }

        public static bool operator >=(SequentialGuid value1, SequentialGuid value2)
        {
            return value1.CompareTo(value2) >= 0;
        }

        public static bool operator <=(Guid value1, SequentialGuid value2)
        {
            return value1.CompareTo(value2) <= 0;
        }

        public static bool operator >=(Guid value1, SequentialGuid value2)
        {
            return value1.CompareTo(value2) >= 0;
        }

        public static bool operator <=(SequentialGuid value1, Guid value2)
        {
            return value1.CompareTo(value2) <= 0;
        }

        public static bool operator >=(SequentialGuid value1, Guid value2)
        {
            return value1.CompareTo(value2) >= 0;
        }

        #endregion

        #region Equality Operators

        public static bool operator ==(SequentialGuid value1, SequentialGuid value2)
        {
            return value1.CompareTo(value2) == 0;
        }

        public static bool operator !=(SequentialGuid value1, SequentialGuid value2)
        {
            return !(value1 == value2);
        }

        public static bool operator ==(Guid value1, SequentialGuid value2)
        {
            return value1.CompareTo(value2) == 0;
        }

        public static bool operator !=(Guid value1, SequentialGuid value2)
        {
            return !(value1 == value2);
        }

        public static bool operator ==(SequentialGuid value1, Guid value2)
        {
            return value1.CompareTo(value2) == 0;
        }

        public static bool operator !=(SequentialGuid value1, Guid value2)
        {
            return !(value1 == value2);
        }

        #endregion

        #region CompareTo

        public int CompareTo(object obj)
        {
            if (obj is SequentialGuid)
            {
                return CompareTo((SequentialGuid)obj);
            }
            if (obj is Guid)
            {
                return CompareTo((Guid)obj);
            }
            throw new ArgumentException("Parameter is not of the rigt type");
        }

        public int CompareTo(SequentialGuid other)
        {
            return CompareTo(other.m_guidValue);
        }

        public int CompareTo(Guid other)
        {
            return CompareImplementation(m_guidValue, other);
        }

        private static readonly int[] m_indexOrderingHighLow =
        { 10, 11, 12, 13, 14, 15, 8, 9, 7, 6, 5, 4, 3, 2, 1, 0 };

        private static int CompareImplementation(Guid left, Guid right)
        {
            var leftBytes = left.ToByteArray();
            var rightBytes = right.ToByteArray();

            return m_indexOrderingHighLow.Select(i => leftBytes[i].CompareTo(rightBytes[i]))
            .FirstOrDefault(r => r != 0);
        }

        #endregion

        #region Equals

        public override bool Equals(Object obj)
        {
            if (obj is SequentialGuid || obj is Guid)
            {
                return CompareTo(obj) == 0;
            }

            return false;
        }

        public bool Equals(SequentialGuid other)
        {
            return CompareTo(other) == 0;
        }

        public bool Equals(Guid other)
        {
            return CompareTo(other) == 0;
        }

        public override int GetHashCode()
        {
            return m_guidValue.GetHashCode();
        }

        #endregion

        #region Conversion operators

        public static implicit operator Guid(SequentialGuid value)
        {
            return value.m_guidValue;
        }

        public static explicit operator SequentialGuid(Guid value)
        {
            return new SequentialGuid(value);
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            var roundedCreatedDateTime = Round(CreatedDateTime, TimeSpan.FromMilliseconds(1));
            return $"{m_guidValue} ({roundedCreatedDateTime:yyyy-MM-dd HH:mm:ss.fff})";
        }

        private static DateTime Round(DateTime dateTime, TimeSpan interval)
        {
            var halfIntervalTicks = (interval.Ticks + 1) >> 1;

            return dateTime.AddTicks(halfIntervalTicks -
            ((dateTime.Ticks + halfIntervalTicks) % interval.Ticks));
        }

        #endregion
    }
}
