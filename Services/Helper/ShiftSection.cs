namespace AttendanceManagementPayrollSystem.Services.Helper
{
    public class ShiftSection
    {
        public readonly static TimeSpan ClockDuration = new TimeSpan(0, 15, 0);

        private TimeSpan checkinStart;
        private TimeSpan shiftStart;
        private TimeSpan shiftEnd;
        private TimeSpan checkoutEnd;

        public readonly decimal ExpectedWorkUnits;
        public readonly decimal ExpectedWorkHours;

        private TimeSpan checkedIn;
        private TimeSpan firstInshiftCheck;
        private TimeSpan lastInshiftCheck;
        private TimeSpan checkedOut;

        public List<TimeSpan> Log;

        public TimeSpan GetCheckIn()
        {
            if (checkedIn != impossibleTimeSpan) return checkedIn;
            else if (firstInshiftCheck != impossibleTimeSpan) return firstInshiftCheck;
            else return impossibleTimeSpan;
        }

        public TimeSpan GetCheckOut()
        {
            if (checkedOut != impossibleTimeSpan) return checkedOut;

            else if (lastInshiftCheck != impossibleTimeSpan && (checkedIn != impossibleTimeSpan || firstInshiftCheck != lastInshiftCheck)) return lastInshiftCheck;
            else return impossibleTimeSpan;
        }

        private static readonly TimeSpan impossibleTimeSpan = TimeSpan.FromHours(-20);

        public override string ToString()
        {
            return $"{shiftStart:hh\\:mm} - {shiftEnd:hh\\:mm}";
        }

        public ShiftSection(TimeSpan start, TimeSpan end, decimal expectedWorkUnits, decimal expectedWorkHours) 
        {
            this.checkinStart = start.Subtract(ClockDuration);
            this.shiftStart = start;
            this.shiftEnd = end;
            this.checkoutEnd = end.Add(ClockDuration);

            this.ExpectedWorkUnits = expectedWorkUnits;
            this.ExpectedWorkHours = expectedWorkHours;

            checkedIn = impossibleTimeSpan;
            firstInshiftCheck = impossibleTimeSpan;
            lastInshiftCheck = impossibleTimeSpan;
            checkedOut = impossibleTimeSpan;

            Log = new();
        }

        public bool IsCheckInSection(TimeSpan snap)
        {
            bool result = false;
            if (checkinStart <= snap && snap <= shiftStart)
            {
                checkedIn = snap;
                result = true;
            }
            else if (shiftEnd <= snap && snap <= checkoutEnd)
            {
                checkedOut = snap;
                result = true;
            }
            else if (shiftStart < snap && snap < shiftEnd)
            {
                if (firstInshiftCheck == impossibleTimeSpan ||  snap < firstInshiftCheck)
                {
                    firstInshiftCheck = new TimeSpan(snap.Hours, snap.Minutes, snap.Seconds);
                    result = true;
                }
                if (lastInshiftCheck == impossibleTimeSpan || lastInshiftCheck < snap)
                {
                    lastInshiftCheck = new TimeSpan(snap.Hours, snap.Minutes, snap.Seconds);
                    result = true;
                }
            }

            if (result) { Log.Add(snap); }
            return result;
        }

        public decimal CalculateWorkUnits()
        {
            if (checkedIn != impossibleTimeSpan && checkedOut != impossibleTimeSpan)
            {
                return ExpectedWorkUnits;
            }
            else if (checkedIn != impossibleTimeSpan && lastInshiftCheck != impossibleTimeSpan)
            {
                decimal ratio = (decimal)(lastInshiftCheck - shiftStart).TotalMinutes / (decimal)(shiftEnd - shiftStart).TotalMinutes;
                return ratio * ExpectedWorkUnits;
            }
            else if (checkedOut != impossibleTimeSpan && firstInshiftCheck != impossibleTimeSpan)
            {
                decimal ratio2 = (decimal)(shiftEnd - firstInshiftCheck).TotalMinutes / (decimal)(shiftEnd - shiftStart).TotalMinutes;
                return ratio2 * ExpectedWorkUnits;
            }
            else if (firstInshiftCheck != impossibleTimeSpan && lastInshiftCheck != impossibleTimeSpan)
            {
                decimal ratio3 = (decimal)(lastInshiftCheck - firstInshiftCheck).TotalMinutes / (decimal)(shiftEnd - shiftStart).TotalMinutes;
                return ratio3 * ExpectedWorkUnits;
            }
            else return 0m;
        }

        public decimal CalculateWorkHours()
        {
            if (checkedIn != impossibleTimeSpan && checkedOut != impossibleTimeSpan)
            {
                return ExpectedWorkHours;
            }
            else if (checkedIn != impossibleTimeSpan && lastInshiftCheck != impossibleTimeSpan)
            {
                return (decimal)(lastInshiftCheck - shiftStart).TotalHours;
            }
            else if (checkedOut != impossibleTimeSpan && firstInshiftCheck != impossibleTimeSpan)
            {
                return (decimal)(shiftEnd - firstInshiftCheck).TotalHours;
            }
            else if (firstInshiftCheck != impossibleTimeSpan && lastInshiftCheck != impossibleTimeSpan)
            {
                return (decimal)(lastInshiftCheck - firstInshiftCheck).TotalHours;
            }
            else return 0m;
        }
    
        public string ToLogString()
        {
            return string.Join("|", Log.Select(t => t.ToString(@"hh\:mm")));
        }

        public string GetDescription()
        {
            if (checkedIn != impossibleTimeSpan && checkedOut != impossibleTimeSpan)
            {
                return "Đủ giờ";
            }
            else if (checkedIn != impossibleTimeSpan && checkedOut == impossibleTimeSpan && lastInshiftCheck != impossibleTimeSpan)
            {
                return "Về sớm";
            }
            else if (checkedIn == impossibleTimeSpan && checkedOut != impossibleTimeSpan && firstInshiftCheck != impossibleTimeSpan)
            {
                return "Vào trễ";
            }
            else if (firstInshiftCheck != impossibleTimeSpan && lastInshiftCheck != impossibleTimeSpan
                    && firstInshiftCheck != lastInshiftCheck)
            {
                return "Vào trễ + Về sớm";
            }
            else if (checkedIn != impossibleTimeSpan && checkedOut == impossibleTimeSpan)
            {
                return "Chỉ vào";
            }
            else if (checkedIn == impossibleTimeSpan && checkedOut != impossibleTimeSpan)
            {
                return "Chỉ ra";
            }
            else if (firstInshiftCheck != impossibleTimeSpan)
            {
                return "Thiếu chấm";
            }
            {
                return "Không chấm công";
            }
        }
    }
}
