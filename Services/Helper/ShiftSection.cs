namespace AttendanceManagementPayrollSystem.Services.Helper
{
    public class ShiftSection
    {
        public readonly static TimeSpan ClockDuration = new TimeSpan(0, 15, 0);

        private TimeSpan checkinStart;
        private TimeSpan shiftStart;
        private TimeSpan shiftEnd;
        private TimeSpan checkoutEnd;

        public readonly decimal ExpectedWorkhour;

        private bool checkedIn = false;
        private TimeSpan firstInshiftCheck;
        private TimeSpan lastInshiftCheck;
        private bool checkedOut = false;

        private static readonly TimeSpan impossibleTimeSpan = TimeSpan.FromHours(-20);

        public ShiftSection(TimeSpan start, TimeSpan end, decimal expectedWorkhour) 
        {
            this.checkinStart = start.Subtract(ClockDuration);
            this.shiftStart = start;
            this.shiftEnd = end;
            this.checkoutEnd = end.Add(ClockDuration);

            this.ExpectedWorkhour = expectedWorkhour;

            firstInshiftCheck = impossibleTimeSpan;
            lastInshiftCheck = impossibleTimeSpan;
        }

        public bool IsCheckInSection(TimeSpan snap)
        {
            if (checkinStart <= snap && snap <= shiftStart)
            {
                checkedIn = true;
                return true;
            }
            else if (shiftEnd <= snap && snap <= checkoutEnd)
            {
                checkedOut = true;
                return true;
            }
            else if (shiftStart < snap && snap < shiftEnd)
            {
                bool result = false;
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
                if (result) return true;
            }

            return false;
        }
        

        public decimal CalculateWorkhour()
        {
            if (checkedIn && checkedOut)
            {
                return ExpectedWorkhour;
            }
            else if (checkedIn && lastInshiftCheck != impossibleTimeSpan)
            {
                decimal ratio = (decimal)(lastInshiftCheck - shiftStart).TotalMinutes / (decimal)(shiftEnd - shiftStart).TotalMinutes;
                return ratio * ExpectedWorkhour;
            }
            else if (checkedOut && firstInshiftCheck != impossibleTimeSpan)
            {
                decimal ratio2 = (decimal)(shiftEnd - firstInshiftCheck).TotalMinutes / (decimal)(shiftEnd - shiftStart).TotalMinutes;
                return ratio2 * ExpectedWorkhour;
            }
            else return 0m;
        }
    }
}
