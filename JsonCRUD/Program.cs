using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


namespace JsonCRUD
{
    static class Program
    {
        /// <summary>
        /// АТХ код.
        /// </summary>
        private class AtcCodes
        {
            /// <summary>
            /// Код АТХ.
            /// </summary>
            public string Code { get; }

            /// <summary>
            /// Русское название.
            /// </summary>
            public string RusName { get; }

            /// <summary>
            /// Английское название.
            /// </summary>
            public string EngName { get; }

            /// <summary>
            /// Конструктор класса.
            /// </summary>
            /// <param name="jObject">Объект класса JObject.</param>
            public AtcCodes(JObject jObject)
            {
                if (jObject == null) return;
                Code = jObject["products"][0]?["atcCodes"]?[0]?.Value<string>("code");
                RusName = jObject["products"][0]?["atcCodes"]?[0]?.Value<string>("rusName");
                EngName = jObject["products"][0]?["atcCodes"]?[0]?.Value<string>("engName");
            }
        }
        
        /// <summary>
        /// Молекула в составе лекарства.
        /// </summary>
        private class Molecule
        {
            /// <summary>
            /// Английское название молекулы.
            /// </summary>
            public string LatName { get; }

            /// <summary>
            /// Русское название молекулы.
            /// </summary>
            public string RusName { get; }

            /// <summary>
            /// Английское название документа про производство лекарства.
            /// </summary>
            public string GnParent { get; }

            /// <summary>
            /// Русское название документа про производство лекарства.
            /// </summary>
            public string Description { get; }

            /// <summary>
            /// Конструктор класса.
            /// </summary>
            /// <param name="jObject">Объект класса JObject.</param>
            /// <param name="index">Номер молекулы из Json-файла.</param>
            public Molecule(JObject jObject, int index)
            {
                LatName = 
                    jObject["products"][0]?["moleculeNames"]?[index]?["molecule"]?.Value<string>("latName");
                RusName = 
                    jObject["products"][0]?["moleculeNames"]?[index]?["molecule"]?.Value<string>("rusName");
                GnParent = 
                    jObject["products"][0]?["moleculeNames"]?[index]?["molecule"]?["GNParent"]?.Value<string>("GNParent");
                Description = 
                    jObject["products"][0]?["moleculeNames"]?[index]?["molecule"]?["GNParent"]?.Value<string>("description");
            }
        }

        /// <summary>
        /// Список молекул лекарства.
        /// </summary>
        private class MoleculeNames
        {
            /// <summary>
            /// Список молекул в лекарстве.
            /// </summary>
            public List<Molecule> Molecule = new List<Molecule>();

            /// <summary>
            /// Конструктор класса.
            /// </summary>
            /// <param name="jObject">Объект класса JObject.</param>
            public MoleculeNames(JObject jObject)
            {
                var count = jObject["products"]?[0]?["moleculeNames"].Count();
                for (int index = 0; index < count; index++)
                {
                    if (Molecule != null) Molecule.Add(new Molecule(jObject, index));
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private class Products
        {
            private AtcCodes _codes;
            private string _phthgroups;
            private string _clPhGroups;
            private MoleculeNames _moleculeName;
            private string _rusName;
            private string _engName;
            private string _zipInfo;
            private string _composition;
            private Companies _companies;
            private Document _document;
        }

        /// <summary>
        /// 
        /// </summary>
        private class Company
        {
            private string _name;
            private string _gddbName;
            private string _country;
        }

        /// <summary>
        /// 
        /// </summary>
        private class Companies
        {
            private Company _company;
        }

        /// <summary>
        /// 
        /// </summary>
        private class Nozologies
        {
            private string _name;
        }

        /// <summary>
        /// 
        /// </summary>
        private class Document
        {
            private string _storageCondition;
            private string _storageTime;
            private List<Nozologies>  _nozologies;
            private string _phInfluence;
            private string _phKinetics;
            private string _dosage;
            private string _overDosage;
            private string _interaction;
            private string _lactation;
            private string _sideEffects;
            private string _indication;
            private string _contraIndication;
            private string _specialInstruction;
            private string _pregnancyUsing;
            private string _nursingUsing;
            private string _renalInsuf;
            private string _renalInsufUsing;
            private string _hepatoInsuf;
            private string _hepatoInsufUsing;
            private string _pharmDelivery;
            private string _elderlyInsuf;
            private string _elderlyInsufUsing;
            private string _childInsuf;
        }

        /// <summary>
        /// Название Json-файла с его расширением.
        /// </summary>
        private const string JsonFile = @"json.json";
        
        /// <summary>
        /// Получение пути до Json-файла.
        /// </summary>
        /// <returns>Путь до Json-файла.</returns>
        private static string GetJsonPath()
        {
            var path = Directory.GetCurrentDirectory();
            // Сокращаем строку, убирая из пути файл Debug и папку Bin.
            path = path.Substring(0, path.Length - 9);
            path += JsonFile;
            return path;
        }

        /// <summary>
        /// Основной метод.
        /// </summary>
        private static void Main()
        {
            var path = GetJsonPath();
            var str = File.ReadAllText(path, Encoding.GetEncoding(1251));
            var jObject = JObject.Parse(str);
            var atc = new AtcCodes(jObject);
            var moleculeNames = new MoleculeNames(jObject);
            Console.WriteLine($"{atc.RusName} / {atc.EngName} \nКод ATX: {atc.Code} \n ");
            Console.WriteLine("Активные вещества: \n ");
            foreach (var molecule in moleculeNames.Molecule)
            {
                Console.WriteLine($"{molecule.RusName} ({molecule.LatName}) " +
                                  $" {molecule.GnParent} {molecule.Description}");
            }
            Console.ReadKey();
        }
    }
}
