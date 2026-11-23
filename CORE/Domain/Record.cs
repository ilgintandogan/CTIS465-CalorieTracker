namespace CORE.Domain
{
    public abstract class Record
    {
        public int Id { get; set; }

        protected Record(int id)
        {
            Id = id;
        }

        protected Record()
        {
        }
    }
}
