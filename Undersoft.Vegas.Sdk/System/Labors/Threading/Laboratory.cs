/*************************************************
   Copyright (c) 2021 Undersoft

   Laboratory.cs
              
   @author: Dariusz Hanc                                                  
   @date: (28.05.2021)                                            
   @licence MIT                                       
 *************************************************/

namespace System.Labors
{
    using System.Collections.Generic;
    using System.Instant;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="InvalidLaborException" />.
    /// </summary>
    public class InvalidLaborException : Exception
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidLaborException"/> class.
        /// </summary>
        /// <param name="message">The message<see cref="string"/>.</param>
        public InvalidLaborException(string message)
            : base(message)
        {
        }

        #endregion
    }

    public class Laboratory
    {
        public Laboratory(LaborMethods _methods, Scope _scope = null)
        {
            methods = new LaborMethods();
            foreach (var rd in _methods)
                methods.Put(rd);

            if (_scope == null)
                scope = new Scope("ThreadLab", new LaborNotes());
            else
                scope = _scope;

            scope.Add("Primary", _methods.AsValues().ToList());
            Scope = scope;
            Notes = Scope.Notes;
            StartLaboring();
        }
        public Laboratory()
        {
            methods = new LaborMethods();
            scope = new Scope("ThreadLab", new LaborNotes());
            Notes = scope.Notes;
        }

        private LaborMethods methods;

        public LaborNotes Notes
        {
            get;
            set;
        }

        private Scope scope;
        public Scope Scope
        {
            get
            {
                return scope;
            }
            set
            {
                scope = value;
            }
        }

        public void Expanse(LaborMethods _labors, Subject _mission)
        {
            if (methods != null && (methods.Count > 0))
                foreach (var cr in _labors)
                    methods.Put(cr);
            else
                methods = _labors;
            if (_mission != null)
            {
                _mission.AddRange(methods.AsValues().ToList());
                Scope.Add(_mission);
            }
        }
        public void Expanse(IDeputy antitem, Subject _mission)
        {
            methods.Put(antitem);
            if (_mission != null)
            {
                _mission.AddRange(methods.AsValues().ToList());
                Scope.Add(_mission);
            }
        }
        public Subject Expanse(LaborMethods _labors, string mission)
        {
            if (methods != null && (methods.Count > 0))
                foreach (var cr in _labors)
                    methods.Put(cr);
            else
                methods = _labors;
            Subject sub = new Subject(mission);
            sub.AddRange(methods.AsValues().ToList());
            Scope.Add(sub);
            return sub;
        }
        public Subject Expanse(IDeputy antitem, string mission)
        {
            methods.Put(antitem);
            Subject sub = new Subject(mission);
            sub.AddRange(methods.AsValues().ToList());
            Scope.Add(sub);
            return sub;
        }

        public void StartLaboring()
        {
            RunLaborators();
        }
        public void RunLaborators()
        {
            foreach (Subject mission in scope.Subjects.AsValues())
            {
                if (mission.Visor == null)
                {
                    mission.Visor = new Laborator(mission);
                }
                if (!mission.Visor.Ready)
                {
                    mission.Visor.CreateLaborers();
                }
            }
        }

        public void Elaborate(string workerName, params object[] input)
        {
            List<Labor> workerLabors = Scope.Subjects
                .AsValues().Where(m => m.Labors.ContainsKey(workerName))
                    .SelectMany(w => w.Labors.AsValues()).ToList();
            foreach (Labor objc in workerLabors)
                objc.Execute(input);
        }
        public void Elaborate(IDictionary<string, object[]> workersAndInputs)
        {
            foreach (KeyValuePair<string, object[]> worker in workersAndInputs)
            {
                object input = worker.Value;
                string workerName = worker.Key;
                List<Labor> workerLabors = Scope.Subjects.AsValues().Where(m => m.Labors.ContainsKey(workerName)).SelectMany(w => w.Labors.AsValues()).ToList();
                foreach (Labor objc in workerLabors)
                    objc.Execute(input);
            }
        }

        public Subject this[string missionName]
        {
            get
            {
                return Scope[missionName];
            }
        }

    }
}
