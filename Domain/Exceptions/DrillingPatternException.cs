namespace Domain.Exceptions
{
    public class DrillingPatternException : Exception
    {
        public DrillingPatternException(string message) : base(message) { }
        
        public DrillingPatternException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
    
    public class InvalidCoordinatesException : DrillingPatternException
    {
        public InvalidCoordinatesException(double x, double y) 
            : base($"Invalid coordinates: ({x}, {y})") { }
    }
    
    public class DuplicateCoordinatesException : DrillingPatternException
    {
        public DuplicateCoordinatesException(double x, double y) 
            : base($"A drill point already exists at coordinates ({x:F2}, {y:F2})") { }
    }
    
    public class MaxDrillPointsExceededException : DrillingPatternException
    {
        public MaxDrillPointsExceededException(int maxPoints) 
            : base($"Maximum number of drill points ({maxPoints}) exceeded") { }
    }
    
    public class DrillPointNotFoundException : DrillingPatternException
    {
        public DrillPointNotFoundException(string pointId) 
            : base($"Drill point with ID '{pointId}' not found") { }
    }
} 