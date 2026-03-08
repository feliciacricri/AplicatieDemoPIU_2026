namespace LibrarieModele
{
    public class Student
    {
        //constante
        private const char SEPARATOR = ' ';

        // data membră privată
        private int[] note;

        // proprietăți auto-implemented
        public int IdStudent { get; set; } // identificator unic student
        public string Nume { get; set; }
        public string Prenume { get; set; }

        public void SetNote(int[] _note)
        {
            note = new int[_note.Length];
            _note.CopyTo(note, 0);
        }

        public int[] GetNote()
        {
            // returnează o copie pentru a proteja datele interne
            return (int[])note.Clone();
        }

        // constructor implicit
        public Student()
        {
            Nume = string.Empty;
            Prenume = string.Empty;
            note = new int[0];
        }

        // constructor cu parametri
        public Student(int idStudent, string nume, string prenume)
        {
            IdStudent = idStudent;
            Nume = nume;
            Prenume = prenume;
            note = new int[0];
        }

        public string Info()
        {
            string sNote = string.Empty;
            if (note != null)
            {
                sNote = string.Join(SEPARATOR.ToString(), note);
            }

            string info = $"Id:{IdStudent} Nume:{Nume ?? "NECUNOSCUT"} Prenume:{Prenume ?? "NECUNOSCUT"}  Note: {sNote}";
            return info;
        }
    }
}
