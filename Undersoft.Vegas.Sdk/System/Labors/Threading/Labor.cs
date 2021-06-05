/*************************************************
   Copyright (c) 2021 Undersoft

   Labor.cs
              
   @author: Dariusz Hanc                                                  
   @date: (28.05.2021)                                            
   @licence MIT                                       
 *************************************************/

namespace System.Labors
{
    using System.Instant;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Uniques;


    public class Labor : Task<object>, IDeputy
    {
        public IUnique Empty => Ussn.Empty;

        public Labor(string name, IDeputy method) : base(() => method.Execute())
        {
            Name = name;
            Laborer = new Laborer(name, method);
            Laborer.Labor = this;
            Box = new NoteBox(Laborer.LaborerName);
            Box.Labor = this;

            SerialCode = new Ussn(method.UniqueKey, 0, 0, 0, 0, DateTime.Now.ToBinary());
        }
        public Labor(Laborer laborer) : base(() => laborer.Work.Execute())
        {
            Name = laborer.LaborerName;
            Laborer = laborer;
            Laborer.Labor = this;
            Box = new NoteBox(Laborer.LaborerName);
            Box.Labor = this;

            SerialCode = new Ussn(laborer.Work.UniqueKey, 0, 0, 0, 0, DateTime.Now.ToBinary());

        }

        public string Name { get; set; }

        public Laborer Laborer { get; set; }

        public Subject Subject { get; set; }

        public Scope Scope { get; set; }

        public NoteBox Box { get; set; }

        public object[] ParameterValues
        {
            get => Laborer.Work.ParameterValues;
            set => Laborer.Work.ParameterValues = value;
        }
        public object this[int fieldId] { get => ParameterValues[fieldId]; set => ParameterValues[fieldId] = value; }
        public object this[string propertyName]
        {
            get
            {
                for (int i = 0; i < Parameters.Length; i++)
                    if (Parameters[i].Name == propertyName)
                        return ParameterValues[i];
                return null;
            }
            set
            {
                for (int i = 0; i < Parameters.Length; i++)
                    if (Parameters[i].Name == propertyName)
                        ParameterValues[i] = value;
            }
        }

        public MethodInfo Info
        {
            get => Laborer.Work.Info;
            set => Laborer.Work.Info = value;
        }

        public ParameterInfo[] Parameters
        {
            get => Laborer.Work.Parameters;
            set => Laborer.Work.Parameters = value;
        }
        public object[] ValueArray { get => ParameterValues; set => ParameterValues = value; }

        public Ussn SerialCode
        {
            get;
            set;
        }

        public ulong UniqueKey { get => SerialCode.UniqueKey; set => SerialCode.SetUniqueKey(value); }

        public ulong UniqueSeed { get => SerialCode.UniqueSeed; set => SerialCode.SetUniqueSeed(value); }

        public void Elaborate(params object[] input)
        {
            Laborer.Input = input;
            this.Subject.Visor.Elaborate(Laborer);
        }

        public object Execute(params object[] parameters)
        {
            this.Elaborate(parameters);
            return null;
        }

        public byte[] GetBytes()
        {
            return Laborer.Work.GetBytes();
        }
        public byte[] GetUniqueBytes()
        {
            return SerialCode.GetUniqueBytes();
        }

        public bool Equals(IUnique other)
        {
            return SerialCode.Equals(other);
        }
        public int CompareTo(IUnique other)
        {
            return SerialCode.CompareTo(other);
        }       
    }
}
