namespace OSP.Common.Domain.Tables
{
    // Define a generic model class with separate data types for Parameter1 and Parameter2
    public class GenericModel<T1, T2>
    {
        public T1 Parameter1 { get; set; }
        public T2 Parameter2 { get; set; }

        // Constructor to initialize the model with parameters
        public GenericModel(T1 parameter1, T2 parameter2)
        {
            Parameter1 = parameter1;
            Parameter2 = parameter2;
        }

        // Add any methods or properties related to the model here
    }
}
