using System.Collections;
using LibrarieModele.Enums;

namespace LibrarieModele
{
    public class Student
    {
        //constante
        private const char SEPARATOR_PRINCIPAL_FISIER = ';';
        private const char SEPARATOR_SECUNDAR_FISIER = ' ';
        private const char SEPARATOR_DISCIPLINE_FISIER = ',';
        private const bool SUCCES = true;
        public const int NOTA_MINIMA = 1;
        public const int NOTA_MAXIMA = 10;

        private const int ID = 0;
        private const int NUME = 1;
        private const int PRENUME = 2;
        private const int NOTE = 3;
        private const int PROGRAM = 4;
        private const int DISCIPLINE = 5;
        private const int FORMA_FINANTARE = 6;


        // data membră privată
        private int[] note;

        // proprietăți auto-implemented
        public int IdStudent { get; set; } // identificator unic student
        public string Nume { get; set; }
        public string Prenume { get; set; }
        public ProgramStudiu ProgramSTD { get; set; }
        public List<string> Discipline { get; set; }
        public string FormaFinantare { get; set; }

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

        // proprietate de tip read-only folosita pentru afisare in DataGrid
        public string NoteAfisare => note != null ? string.Join(" ", note) : string.Empty;

        public string DisciplineAfisare => Discipline != null ? string.Join(", ", Discipline) : string.Empty;

        // constructor implicit
        public Student()
        {
            Nume = string.Empty;
            Prenume = string.Empty;
            note = new int[0];
            // Optiunea implicita este Calculatoare
            ProgramSTD = ProgramStudiu.Calculatoare;
            Discipline = new List<string>();
            FormaFinantare = string.Empty;
        }

        // constructor cu parametri
        public Student(int idStudent, string nume, string prenume)
        {
            IdStudent = idStudent;
            Nume = nume;
            Prenume = prenume;
            note = new int[0];
            ProgramSTD = ProgramStudiu.Calculatoare;
            Discipline = new List<string>();
            FormaFinantare = string.Empty;
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

            // pentru compatibilitate cu fisierele existente, daca lipseste programul se seteaza valoare implicita
            if (dateFisier.Length > PROGRAM && Enum.TryParse(dateFisier[PROGRAM], out ProgramStudiu program))
            {
                this.ProgramSTD = program;
            }
            else
            {
                this.ProgramSTD = ProgramStudiu.Calculatoare;
            }

            if (dateFisier.Length > DISCIPLINE && !string.IsNullOrWhiteSpace(dateFisier[DISCIPLINE]))
            {
                this.Discipline = dateFisier[DISCIPLINE]
                    .Split(SEPARATOR_DISCIPLINE_FISIER, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();
            }
            else
            {
                this.Discipline = new List<string>();
            }

            if (dateFisier.Length > FORMA_FINANTARE)
            {
                this.FormaFinantare = dateFisier[FORMA_FINANTARE];
            }
            else
            {
                this.FormaFinantare = string.Empty;
            }
        }


        public string Info()
        {
            string sNote = string.Empty;
            if (note != null)
            {
                sNote = string.Join(SEPARATOR_SECUNDAR_FISIER.ToString(), note);
            }

            string sDiscipline = Discipline != null ? string.Join(", ", Discipline) : string.Empty;

            string info = $"Id:{IdStudent} Nume:{Nume ?? "NECUNOSCUT"} Prenume:{Prenume ?? "NECUNOSCUT"}  Note: {sNote} Program: {ProgramSTD} Discipline: {sDiscipline} Forma finantare: {FormaFinantare}";
            return info;
        }

        public string ConversieLaSirPentruFisier()
        {
            string sNote = string.Empty;
            if (note != null)
            {
                sNote = string.Join(SEPARATOR_SECUNDAR_FISIER.ToString(), note);
            }

            string sDiscipline = Discipline != null ? string.Join(SEPARATOR_DISCIPLINE_FISIER.ToString(), Discipline) : string.Empty;

            string obiectStudentPentruFisier = string.Format("{1}{0}{2}{0}{3}{0}{4}{0}{5}{0}{6}{0}{7}",
                SEPARATOR_PRINCIPAL_FISIER,
                IdStudent.ToString(),
                (Nume ?? " NECUNOSCUT "),
                (Prenume ?? " NECUNOSCUT "),
                sNote,
                ProgramSTD.ToString(),
                sDiscipline,
                FormaFinantare ?? string.Empty);

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
