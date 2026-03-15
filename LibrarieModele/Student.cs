using System.Collections;

namespace LibrarieModele
{
    public class Student
    {
        //constante
        private const char SEPARATOR_PRINCIPAL_FISIER = ';';
        private const char SEPARATOR_SECUNDAR_FISIER = ' ';
        private const bool SUCCES = true;
        public const int NOTA_MINIMA = 1;
        public const int NOTA_MAXIMA = 10;

        private const int ID = 0;
        private const int NUME = 1;
        private const int PRENUME = 2;
        private const int NOTE = 3;


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

        //constructor cu un singur parametru de tip string care reprezinta o linie dintr-un fisier text
        public Student(string linieFisier)
        {
            string[] dateFisier = linieFisier.Split(SEPARATOR_PRINCIPAL_FISIER);

            //ordinea de preluare a campurilor este data de ordinea in care au fost scrise in fisier prin apelul implicit al metodei ConversieLaSir_PentruFisier()
            this.IdStudent = Convert.ToInt32(dateFisier[ID]);
            this.Nume = dateFisier[NUME];
            this.Prenume = dateFisier[PRENUME];
            ExtrageNote(dateFisier[NOTE], SEPARATOR_SECUNDAR_FISIER);
        }


        public string Info()
        {
            string sNote = string.Empty;
            if (note != null)
            {
                sNote = string.Join(SEPARATOR_SECUNDAR_FISIER.ToString(), note);
            }

            string info = $"Id:{IdStudent} Nume:{Nume ?? "NECUNOSCUT"} Prenume:{Prenume ?? "NECUNOSCUT"}  Note: {sNote}";
            return info;
        }

        public string ConversieLaSirPentruFisier()
        {
            string sNote = string.Empty;
            if (note != null)
            {
                sNote = string.Join(SEPARATOR_SECUNDAR_FISIER.ToString(), note);
            }

            string obiectStudentPentruFisier = string.Format("{1}{0}{2}{0}{3}{0}{4}",
                SEPARATOR_PRINCIPAL_FISIER,
                IdStudent.ToString(),
                (Nume ?? " NECUNOSCUT "),
                (Prenume ?? " NECUNOSCUT "),
                sNote);

            return obiectStudentPentruFisier;
        }


        public void ExtrageNote(string sirNote, char delimitator = ' ')
        {
            List<int> listaNote = new List<int>();

            foreach (var element in sirNote.Split(delimitator))
            {
                if (int.TryParse(element, out int nota) && ValideazaNota(nota) == SUCCES)
                {
                    listaNote.Add(nota);
                }
            }

            note = listaNote.ToArray();
        }

        private bool ValideazaNota(int nota)
        {
            if (nota >= NOTA_MINIMA && nota <= NOTA_MAXIMA)
            {
                return true;
            }

            return false;
        }
    }
}
