/// <summary>
///A la hora de crear la "Base de datos" me base en la informacion de INE (instituto nacional de estadistica) de 2018 para recoger informacion
///siendo que del total de la poblacion, solamente 4.746.651 se enfermaron ese año representando aproximadamente 10.11% apartir de este punto
///extrapole a base de ese mismo estudio para sacar el porcentaje de los casos de cada tipo de enfermedad y sus "sintomas" mas habituales
///para poder dar una respuesta en caso de que mi sistema experto no pudiera dar una respuesta, y dentro del sistema experto he creado categorys
///y un par de enfermedades de cada para no extender el codigo en exceso
/// </summary>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Linq.Expressions;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Threading;
using chen0040.ExpertSystem;



namespace ConsoleApp1
{
    /// <summary>
    /// Clase que representa una enfermedad.
    /// </summary>
    public class Illnes
    {
        /// <summary>
        /// Nombre de la enfermedad.
        /// </summary>
        public string nameillness { get; set; }

        /// <summary>
        /// Lista de síntomas asociados a la enfermedad.
        /// </summary>
        public List<string> symptoms { get; set; }

        /// <summary>
        /// Probabilidades asociadas a la enfermedad.
        /// </summary>
        public double probabilities { get; set; }

        /// <summary>
        /// Probabilidades de que tenga esa enfermedad despues del teorema de bayes
        /// </summary>

        public double bayes { get; set; }

    }

    class Program
    {
        public static Illnes TheoremBayes(List<Illnes> bbdd, List<string> tmp_sym)
        {
            //Segun los datos del INE en 2018 el 10,11% fue al hospital por alguna cuestion por tanto, 89.89% no fue. Generamos las constantes
            const double go_hospital = 0.1011, no_hospital = 0.8989;
            //Por otro lado tenemos que de los que no fueron al medico, el 37.35 % tenian sintomas por tanto el 62.65 % estaban bien. Generamos las constantes
            const double no_hospital_enf = 0.3735;
            //De los que fueron al hospital solo el 0.0305% se dieron de alta sin diagnostico (los tomaremos como no enfermos), por tanto
            //el 99.96 % si que tenian algun cuadro medico, generemos las variables
            const double go_hospital_enf = 0.9996;
            //Y por ultimo hay 2 porcentajes que tendre en cuenta en la ecuacion, el porcentaje de los casos sobre un tipo de enfermedad
            //(virica, infecciosa) que he calculado del numero de casos sobre los casos de enfermos que hubo y un porcentaje que voy a utilizar
            //para intentar cuadrar la enfermedad del caso en cuestion, es decir, de los sintomas que me diga "el paciente" intentare sacar el 
            //porcentaje de la cantidad de sintomas con respecto al total y posteriormente contarlo en mi teorema de bayes
            Illnes tmp_illnes = null;
            int amount = 0, amountElement;
            IEnumerable<string> commonElements;

            foreach (var illnes in bbdd)
            {

                commonElements = illnes.symptoms.Intersect(tmp_sym);
                amountElement = commonElements.Count();
                if (amountElement > amount)
                {
                    tmp_illnes = illnes;
                    amount = amountElement;
                }
            }
            if (tmp_illnes is null)
            {
                Illnes healthy = new Illnes();
                healthy.symptoms = new List<string>();
                healthy.probabilities = 0;
                healthy.bayes = (amount * tmp_sym.Count()) / 100;
                healthy.nameillness = "Healthy";
                return healthy;
            }
            else {
                double perct = ((double) amount / tmp_illnes.symptoms.Count());
                double nume = (double) (go_hospital * go_hospital_enf * tmp_illnes.probabilities * perct);
                double denomi = (double) (no_hospital * no_hospital_enf * tmp_illnes.probabilities * perct);
                double result_bayes = (double)nume / (nume + denomi);
                tmp_illnes.bayes = Math.Round(result_bayes, 4);

                return tmp_illnes;
            }
            
        }
        /// <summary>
        /// Crea la base de datos de enfermedades.
        /// </summary>
        /// <returns>La lista de enfermedades creada.</returns>
        public static List<Illnes> CreateDDBB()
        {
            List<Illnes> options_illnes = new List<Illnes>();

            // Crear enfermedades y las agregas a la lista
            Illnes allergy = new Illnes();
            allergy.nameillness = "Allergy";
            allergy.symptoms = new List<string> { "cough", "sneezing", "pain_discomfort_itching", "congestion" };
            allergy.probabilities = 0.1513;

            options_illnes.Add(allergy);

            Illnes infectious = new Illnes();
            infectious.nameillness = "infectious";
            infectious.symptoms = new List<string> { "cough", "sneezing", "fever", "snot", "sore_throat" };
            infectious.probabilities = 0.023043;

            options_illnes.Add(infectious);

            Illnes Ear = new Illnes();
            Ear.nameillness = "Ear";
            Ear.symptoms = new List<string> { "lost_audition", "tinnitus", "dizziness", "vertigo", "prob_coordinate_balance", "Acúfenos", "press_ears" };
            Ear.probabilities = 0.029509;

            options_illnes.Add(Ear);

            Illnes Respiratory = new Illnes();
            Respiratory.nameillness = "Respiratory";
            Respiratory.symptoms = new List<string> { "cough", "dyspnoea", "wheezing", "fatigue", "chest_pain", "hoarseness" };
            Respiratory.probabilities = 0.1206;

            options_illnes.Add(Respiratory);

            Illnes Autoimmune = new Illnes();
            Autoimmune.nameillness = "Autoimmune";
            Autoimmune.symptoms = new List<string> {"neurological_problems", "changes_skin_hair", "gastrointestinal_disorders","skin_inflammation",
            "articular_muscle_pain", "fatigue"};
            Autoimmune.probabilities = 0.0312;

            options_illnes.Add(Autoimmune);

            Illnes Blood = new Illnes();
            Blood.nameillness = "Sangre";
            Blood.symptoms = new List<string> {"fatigue", "pallor", "without_breath","bloodness_bruises", "articular_muscle_pain",
                "pain_discomfort_itching"};
            Blood.probabilities = 0.009224;

            options_illnes.Add(Blood);

            Illnes Kidney = new Illnes();
            Kidney.nameillness = "Kidney";
            Kidney.symptoms = new List<string> { "changes_urination", "changes_color_urination", "swelling", "fatigue", "weakness", "lumbar_pain", "hypertension" };
            Kidney.probabilities = 0.12044;

            options_illnes.Add(Kidney);

            Illnes Cardiovascular = new Illnes();
            Cardiovascular.nameillness = "Cardiovascular";
            Cardiovascular.symptoms = new List<string> { "chest_pain", "diff_breath", "fatigue", "weakness", "arrhythmias", "dizziness", "swelling" };
            Cardiovascular.probabilities = 0.13222;

            options_illnes.Add(Cardiovascular);

            Illnes Neurological = new Illnes();
            Neurological.nameillness = "Neurological";
            Neurological.symptoms = new List<string> { "dizziness", "vertigo", "headache","weakness", "prob_coordinate_balance", "lost_memory_cog",
                "sensiblity_changes" };
            Neurological.probabilities = 0.1923;

            options_illnes.Add(Neurological);

            Illnes ETS = new Illnes();
            ETS.nameillness = "ETS";
            ETS.symptoms = new List<string> { "abnormal_genital_fluids", "urination_pain", "genital_injuries", "pain_discomfort_itching", "pain_sx" };
            ETS.probabilities = 0.06702;

            options_illnes.Add(ETS);

            Illnes eyes = new Illnes();
            eyes.nameillness = "eyes";
            eyes.symptoms = new List<string> { "blurred_vision", "dry_eye", "eye_redness", "eye_pain", "vision_changes" };
            eyes.probabilities = 0.029509;

            options_illnes.Add(eyes);

            Illnes Digestive = new Illnes();
            Digestive.nameillness = "Digestive";
            Digestive.symptoms = new List<string> { "abdominal_pain", "heartburn_reflux", "pain_difficulty_swallowing", "abdominal_bloating",
                 "nausea_vomit"};
            Digestive.probabilities = 0.12044;

            options_illnes.Add(Digestive);

            Illnes Endocrine = new Illnes();
            Endocrine.nameillness = "Endocrine";
            Endocrine.symptoms = new List<string> { "fatigue", "weakness", "change_weight", "changes_appetite", "changes_urination", "pallor", "change_mood" };
            Endocrine.probabilities = 0.01808;

            options_illnes.Add(Endocrine);

            Illnes Genetics = new Illnes();
            Genetics.nameillness = "Genetics";
            Genetics.symptoms = new List<string> { "development_delay", "birth_abnormalities", "metabolic_disorders", "physical_changes" };
            Genetics.probabilities = 0.031;

            options_illnes.Add(Genetics);

            Illnes OsteoMuscular = new Illnes();
            OsteoMuscular.nameillness = "OsteoMuscular";
            OsteoMuscular.symptoms = new List<string> { "articular_muscle_pain","joint_stiffness","swelling", "bone_malformation","limit_mobility",
                 "weakness","fractures"};
            OsteoMuscular.probabilities = 0.004532;

            options_illnes.Add(OsteoMuscular);

            Illnes Cancer = new Illnes();
            Cancer.nameillness = "Cancer";
            Cancer.symptoms = new List<string> { "cough", "change_weight", "fatigue", "localized_pain", "changes_urination", "pain_difficulty_swallowing" };
            Cancer.probabilities = 0.040023;

            options_illnes.Add(Cancer);

            return options_illnes;
        }

        /// <summary>
        /// Obtiene el motor de inferencia de reglas.
        /// </summary>
        /// <returns>El motor de inferencia de reglas.</returns>
        static RuleInferenceEngine getInferenceEngine()
        {
            RuleInferenceEngine rie = new RuleInferenceEngine();

            //Agrega cada regla (category y subcategorys) al motor de inferencia

            Rule rule = new Rule("Pollen");
            rule.AddAntecedent(new IsClause("category", "Allergys"));
            rule.AddAntecedent(new IsClause("congestion", "Y"));
            rule.AddAntecedent(new IsClause("runny nose", "Y"));
            rule.AddAntecedent(new IsClause("tearing", "Y"));
            rule.AddAntecedent(new IsClause("chest_beeps", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "Polen"));
            rie.AddRule(rule);

            rule = new Rule("Moho");
            rule.AddAntecedent(new IsClause("category", "Allergys"));
            rule.AddAntecedent(new IsClause("tearful_eyes", "Y"));
            rule.AddAntecedent(new IsClause("dry_skin", "Y"));
            rule.AddAntecedent(new IsClause("itch_eyes", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "Moho"));
            rie.AddRule(rule);

            rule = new Rule("Bronchitis");
            rule.AddAntecedent(new IsClause("category", "Infectious"));
            rule.AddAntecedent(new IsClause("snot", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "Bronchitis"));
            rie.AddRule(rule);

            rule = new Rule("Constipated");
            rule.AddAntecedent(new IsClause("category", "Infectious"));
            rule.AddAntecedent(new IsClause("snot", "Y"));
            rule.AddAntecedent(new IsClause("sneezing", "Y"));
            rule.AddAntecedent(new IsClause("sore_throat", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "Constipated"));
            rie.AddRule(rule);

            rule = new Rule("Asma");
            rule.AddAntecedent(new IsClause("category", "Respiratory"));
            rule.AddAntecedent(new IsClause("swelling", "Y"));
            rule.AddAntecedent(new IsClause("diff_breath", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "Asma"));
            rie.AddRule(rule);

            rule = new Rule("EPOC");
            rule.AddAntecedent(new IsClause("category", "Respiratory"));
            rule.AddAntecedent(new IsClause("inhale_tobacco_smoke", "Y"));
            rule.AddAntecedent(new IsClause("diff_breath", "Y"));
            rule.AddAntecedent(new IsClause("snot", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "EPOC"));
            rie.AddRule(rule);

            rule = new Rule("Ménière");
            rule.AddAntecedent(new IsClause("category", "Ear"));
            rule.AddAntecedent(new IsClause("nausea", "Y"));
            rule.AddAntecedent(new IsClause("vomiting", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "Ménière"));
            rie.AddRule(rule);

            rule = new Rule("Otosclerosis");
            rule.AddAntecedent(new IsClause("category", "Ear"));
            rule.AddAntecedent(new IsClause("abnormal_growth_bone_tissue_ear", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "Otosclerosis"));
            rie.AddRule(rule);

            rule = new Rule("LES");
            rule.AddAntecedent(new IsClause("category", "Autoimmune"));
            rule.AddAntecedent(new IsClause("rashes", "Y"));
            rule.AddAntecedent(new IsClause("fever", "Y"));
            rule.AddAntecedent(new IsClause("mouth_ulcers", "Y"));
            rule.AddAntecedent(new IsClause("lost_hair", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "LES"));
            rie.AddRule(rule);

            rule = new Rule("AR");
            rule.AddAntecedent(new IsClause("category", "Autoimmune"));
            rule.AddAntecedent(new IsClause("swelling", "Y"));
            rule.AddAntecedent(new IsClause("joint_stiffness", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "AR"));
            rie.AddRule(rule);

            rule = new Rule("Anemia");
            rule.AddAntecedent(new IsClause("category", "Sangre"));
            rule.AddAntecedent(new IsClause("iron_deficiency", "Y"));
            rule.AddAntecedent(new IsClause("weakness", "Y"));
            rule.AddAntecedent(new IsClause("dizziness", "Y"));
            rule.AddAntecedent(new IsClause("mucous_membrane", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "Anemia"));
            rie.AddRule(rule);

            rule = new Rule("VonWillebrand");
            rule.AddAntecedent(new IsClause("category", "Sangre"));
            rule.AddAntecedent(new IsClause("frequent_nosebleeds", "Y"));
            rule.AddAntecedent(new IsClause("bleeding_gums", "Y"));
            rule.AddAntecedent(new IsClause("gastrointestinal_bleeding", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "VonWillebrand"));
            rie.AddRule(rule);

            rule = new Rule("ERC");
            rule.AddAntecedent(new IsClause("category", "Kidney"));
            rule.AddAntecedent(new IsClause("urination_bleed", "Y"));
            rule.AddAntecedent(new IsClause("kidney_pain", "Y"));
            rule.AddAntecedent(new IsClause("gastrointestinal_bleeding", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "ERC"));
            rie.AddRule(rule);

            rule = new Rule("DiabeticNephropathy");
            rule.AddAntecedent(new IsClause("category", "Kidney"));
            rule.AddAntecedent(new IsClause("protein_urine", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "DiabeticNephropathy"));
            rie.AddRule(rule);

            rule = new Rule("Migraine");
            rule.AddAntecedent(new IsClause("category", "Neurological"));
            rule.AddAntecedent(new IsClause("nausea", "Y"));
            rule.AddAntecedent(new IsClause("vomiting", "Y"));
            rule.AddAntecedent(new IsClause("changes_vision", "Y"));
            rule.AddAntecedent(new IsClause("fatigue", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "Migraine"));
            rie.AddRule(rule);

            rule = new Rule("Parkinson");
            rule.AddAntecedent(new IsClause("category", "Neurological"));
            rule.AddAntecedent(new IsClause("shaking", "Y"));
            rule.AddAntecedent(new IsClause("joint_stiffness", "Y"));
            rule.AddAntecedent(new IsClause("changes_vision", "Y"));
            rule.AddAntecedent(new IsClause("prob_coordinate_balance", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "Parkinson"));
            rie.AddRule(rule);

            rule = new Rule("EAC");
            rule.AddAntecedent(new IsClause("category", "Cardiovascular"));
            rule.AddAntecedent(new IsClause("angina_chest", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "EAC"));
            rie.AddRule(rule);

            rule = new Rule("HeartFailure");
            rule.AddAntecedent(new IsClause("category", "Cardiovascular"));
            rule.AddAntecedent(new IsClause("cough", "Y"));
            rule.AddAntecedent(new IsClause("wheezing", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "HeartFailure"));
            rie.AddRule(rule);

            rule = new Rule("Chlamydia");
            rule.AddAntecedent(new IsClause("category", "ETS"));
            rule.AddAntecedent(new IsClause("abdominal_pain", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "Chlamydia"));
            rie.AddRule(rule);

            rule = new Rule("Gonorrhea");
            rule.AddAntecedent(new IsClause("category", "ETS"));
            rule.AddAntecedent(new IsClause("vaginal_bleeding", "Y"));
            rule.AddAntecedent(new IsClause("testicular_inflammation_pain", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "Gonorrhea"));
            rie.AddRule(rule);

            rule = new Rule("Myopia");
            rule.AddAntecedent(new IsClause("category", "eyes"));
            rule.AddAntecedent(new IsClause("headache", "Y"));
            rule.AddAntecedent(new IsClause("eye_fatigue", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "Myopia"));
            rie.AddRule(rule);

            rule = new Rule("DryEye");
            rule.AddAntecedent(new IsClause("category", "eyes"));
            rule.AddAntecedent(new IsClause("photosensitive", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "DryEye"));
            rie.AddRule(rule);

            rule = new Rule("ERGE");
            rule.AddAntecedent(new IsClause("category", "Digestive"));
            rule.AddAntecedent(new IsClause("chest_pain", "Y"));
            rule.AddAntecedent(new IsClause("sore_throat", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "ERGE"));
            rie.AddRule(rule);

            rule = new Rule("SII");
            rule.AddAntecedent(new IsClause("category", "Digestive"));
            rule.AddAntecedent(new IsClause("diarrhea_constipation", "Y"));
            rule.AddAntecedent(new IsClause("mucus_stool", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "SII"));
            rie.AddRule(rule);

            rule = new Rule("MellitusDiabetes");
            rule.AddAntecedent(new IsClause("category", "Endocrine"));
            rule.AddAntecedent(new IsClause("increased_thirst", "Y"));
            rule.AddAntecedent(new IsClause("blurred_vision", "Y"));
            rule.AddAntecedent(new IsClause("recurrent_infections", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "MellitusDiabetes"));
            rie.AddRule(rule);

            rule = new Rule("Hypothyroidism");
            rule.AddAntecedent(new IsClause("category", "Endocrine"));
            rule.AddAntecedent(new IsClause("constipation", "Y"));
            rule.AddAntecedent(new IsClause("cold", "Y"));
            rule.AddAntecedent(new IsClause("depression", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "Hypothyroidism"));
            rie.AddRule(rule);

            rule = new Rule("Down");
            rule.AddAntecedent(new IsClause("category", "Genetics"));
            rule.AddAntecedent(new IsClause("epicanthal_folds", "Y"));
            rule.AddAntecedent(new IsClause("thyroid_disorders", "Y"));
            rule.AddAntecedent(new IsClause("low_muscle_tone", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "Down"));
            rie.AddRule(rule);

            rule = new Rule("CysticFibrosis");
            rule.AddAntecedent(new IsClause("category", "Genetics"));
            rule.AddAntecedent(new IsClause("diff_breath", "Y"));
            rule.AddAntecedent(new IsClause("male_infertility", "Y"));
            rule.AddAntecedent(new IsClause("delayed_growth_development.", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "CysticFibrosis"));
            rie.AddRule(rule);

            rule = new Rule("Arthritis");
            rule.AddAntecedent(new IsClause("category", "OsteoMuscular"));
            rule.AddAntecedent(new IsClause("inflammation", "Y"));
            rule.AddAntecedent(new IsClause("low_movility", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "Arthritis"));
            rie.AddRule(rule);

            rule = new Rule("HerniatedDisc");
            rule.AddAntecedent(new IsClause("category", "OsteoMuscular"));
            rule.AddAntecedent(new IsClause("pain_back", "Y"));
            rule.AddAntecedent(new IsClause("numbness_extremities", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "HerniatedDisc"));
            rie.AddRule(rule);
            rule = new Rule("Lung");
            rule.AddAntecedent(new IsClause("category", "Cancer"));
            rule.AddAntecedent(new IsClause("diff_breath", "Y"));
            rule.AddAntecedent(new IsClause("hoarseness", "Y"));
            rule.AddAntecedent(new IsClause("weakness", "Y"));
            rule.AddAntecedent(new IsClause("smoker", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "Lung"));
            rie.AddRule(rule);

            rule = new Rule("MammaryGland");
            rule.AddAntecedent(new IsClause("category", "Cancer"));
            rule.AddAntecedent(new IsClause("lumps", "Y"));
            rule.AddAntecedent(new IsClause("BreastChanges", "Y"));
            rule.AddAntecedent(new IsClause("changes_skin_hair", "Y"));
            rule.setConsequent(new IsClause("sub_disease", "MammaryGland"));
            rie.AddRule(rule);

            rule = new Rule("Allergys");
            rule.AddAntecedent(new IsClause("cough", "Y"));
            rule.AddAntecedent(new IsClause("sneezing", "Y"));
            rule.AddAntecedent(new IsClause("pain_discomfort_itching", "Y"));
            rule.AddAntecedent(new IsClause("congestión", "Y"));
            rule.setConsequent(new IsClause("category", "Allergys"));
            rie.AddRule(rule);

            rule = new Rule("Infectious");
            rule.AddAntecedent(new IsClause("cough", "Y"));
            rule.AddAntecedent(new IsClause("sneezing", "Y"));
            rule.AddAntecedent(new IsClause("fever", "Y"));
            rule.AddAntecedent(new IsClause("snot", "Y"));
            rule.AddAntecedent(new IsClause("sore_throat", "Y"));
            rule.setConsequent(new IsClause("category", "Infectious"));
            rie.AddRule(rule);

            rule = new Rule("Respiratory");
            rule.AddAntecedent(new IsClause("cough", "Y"));
            rule.AddAntecedent(new IsClause("dyspnoea", "Y"));
            rule.AddAntecedent(new IsClause("wheezing", "Y"));
            rule.AddAntecedent(new IsClause("fatigue", "Y"));
            rule.AddAntecedent(new IsClause("chest_pain", "Y"));
            rule.AddAntecedent(new IsClause("hoarseness", "Y"));
            rule.setConsequent(new IsClause("category", "Respiratory"));
            rie.AddRule(rule);

            rule = new Rule("Ear");
            rule.AddAntecedent(new IsClause("lost_audition", "Y"));
            rule.AddAntecedent(new IsClause("tinnitus", "Y"));
            rule.AddAntecedent(new IsClause("dizziness", "Y"));
            rule.AddAntecedent(new IsClause("vertigo", "Y"));
            rule.AddAntecedent(new IsClause("prob_coordinate_balance", "Y"));
            rule.AddAntecedent(new IsClause("press_ears", "Y"));
            rule.setConsequent(new IsClause("category", "Ear"));
            rie.AddRule(rule);

            rule = new Rule("Autoimmune");
            rule.AddAntecedent(new IsClause("neurological_problems", "Y"));
            rule.AddAntecedent(new IsClause("changes_skin_hair", "Y"));
            rule.AddAntecedent(new IsClause("gastrointestinal_disorders", "Y"));
            rule.AddAntecedent(new IsClause("skin_inflammation", "Y"));
            rule.AddAntecedent(new IsClause("articular_muscle_pain", "Y"));
            rule.AddAntecedent(new IsClause("fatigue", "Y"));
            rule.setConsequent(new IsClause("category", "Autoimmune"));
            rie.AddRule(rule);


            rule = new Rule("Sangre");
            rule.AddAntecedent(new IsClause("fatigue", "Y"));
            rule.AddAntecedent(new IsClause("pallor", "Y"));
            rule.AddAntecedent(new IsClause("without_breath", "Y"));
            rule.AddAntecedent(new IsClause("bloodness_bruises", "Y"));
            rule.AddAntecedent(new IsClause("articular_muscle_pain", "Y"));
            rule.AddAntecedent(new IsClause("pain_discomfort_itching", "Y"));
            rule.setConsequent(new IsClause("category", "Sangre"));
            rie.AddRule(rule);


            rule = new Rule("Kidney");
            rule.AddAntecedent(new IsClause("changes_urination", "Y"));
            rule.AddAntecedent(new IsClause("changes_color_urination", "Y"));
            rule.AddAntecedent(new IsClause("swelling", "Y"));
            rule.AddAntecedent(new IsClause("fatigue", "Y"));
            rule.AddAntecedent(new IsClause("weakness", "Y"));
            rule.AddAntecedent(new IsClause("hypertension", "Y"));
            rule.setConsequent(new IsClause("category", "Kidney"));
            rie.AddRule(rule);

            rule = new Rule("Neurological");
            rule.AddAntecedent(new IsClause("headache", "Y"));
            rule.AddAntecedent(new IsClause("dizziness", "Y"));
            rule.AddAntecedent(new IsClause("vertigo", "Y"));
            rule.AddAntecedent(new IsClause("weakness", "Y"));
            rule.AddAntecedent(new IsClause("prob_coordinate_balance", "Y"));
            rule.AddAntecedent(new IsClause("lost_memory_cog", "Y"));
            rule.AddAntecedent(new IsClause("sensiblity_changes", "Y"));
            rule.setConsequent(new IsClause("category", "Neurological"));
            rie.AddRule(rule);

            rule = new Rule("Cardiovascular");
            rule.AddAntecedent(new IsClause("chest_pain", "Y"));
            rule.AddAntecedent(new IsClause("diff_breath", "Y"));
            rule.AddAntecedent(new IsClause("fatigue", "Y"));
            rule.AddAntecedent(new IsClause("weakness", "Y"));
            rule.AddAntecedent(new IsClause("arrhythmias", "Y"));
            rule.AddAntecedent(new IsClause("dizziness", "Y"));
            rule.AddAntecedent(new IsClause("swelling", "Y"));
            rule.setConsequent(new IsClause("category", "Cardiovascular"));
            rie.AddRule(rule);

            rule = new Rule("ETS");
            rule.AddAntecedent(new IsClause("rash_sores_other", "Y"));
            rule.AddAntecedent(new IsClause("abnormal_genital_fluids", "Y"));
            rule.AddAntecedent(new IsClause("urination_pain", "Y"));
            rule.AddAntecedent(new IsClause("genital_injuries", "Y"));
            rule.AddAntecedent(new IsClause("pain_discomfort_itching", "Y"));
            rule.AddAntecedent(new IsClause("pain_sx", "Y"));
            rule.setConsequent(new IsClause("category", "ETS"));
            rie.AddRule(rule);

            rule = new Rule("eyes");
            rule.AddAntecedent(new IsClause("blurred_vision", "Y"));
            rule.AddAntecedent(new IsClause("dry_eye", "Y"));
            rule.AddAntecedent(new IsClause("eye_redness", "Y"));
            rule.AddAntecedent(new IsClause("eye_pain", "Y"));
            rule.AddAntecedent(new IsClause("vision_changes", "Y"));
            rule.setConsequent(new IsClause("category", "eyes"));
            rie.AddRule(rule);

            rule = new Rule("Digestive");
            rule.AddAntecedent(new IsClause("abdominal_pain", "Y"));
            rule.AddAntecedent(new IsClause("heartburn_reflux", "Y"));
            rule.AddAntecedent(new IsClause("pain_difficulty_swallowing", "Y"));
            rule.AddAntecedent(new IsClause("abdominal_bloating", "Y"));
            rule.AddAntecedent(new IsClause("nausea_vomit", "Y"));
            rule.setConsequent(new IsClause("category", "Digestive"));
            rie.AddRule(rule);

            rule = new Rule("Endocrine");
            rule.AddAntecedent(new IsClause("fatigue", "Y"));
            rule.AddAntecedent(new IsClause("weakness", "Y"));
            rule.AddAntecedent(new IsClause("change_weight", "Y"));
            rule.AddAntecedent(new IsClause("changes_appetite", "Y"));
            rule.AddAntecedent(new IsClause("changes_urination", "Y"));
            rule.AddAntecedent(new IsClause("pallor", "Y"));
            rule.AddAntecedent(new IsClause("change_mood", "Y"));
            rule.setConsequent(new IsClause("category", "Endocrine"));
            rie.AddRule(rule);

            rule = new Rule("Genetics");
            rule.AddAntecedent(new IsClause("development_delay", "Y"));
            rule.AddAntecedent(new IsClause("birth_abnormalities", "Y"));
            rule.AddAntecedent(new IsClause("metabolic_disorders", "Y"));
            rule.AddAntecedent(new IsClause("physical_changes", "Y"));
            rule.setConsequent(new IsClause("category", "Genetics"));
            rie.AddRule(rule);

            rule = new Rule("OsteoMuscular");
            rule.AddAntecedent(new IsClause("articular_muscle_pain", "Y"));
            rule.AddAntecedent(new IsClause("joint_stiffness", "Y"));
            rule.AddAntecedent(new IsClause("swelling", "Y"));
            rule.AddAntecedent(new IsClause("bone_malformation", "Y"));
            rule.AddAntecedent(new IsClause("limit_mobility", "Y"));
            rule.AddAntecedent(new IsClause("weakness", "Y"));
            rule.AddAntecedent(new IsClause("fractures", "Y"));
            rule.setConsequent(new IsClause("category", "OsteoMuscular"));
            rie.AddRule(rule);

            rule = new Rule("Cancer");
            rule.AddAntecedent(new IsClause("cough", "Y"));
            rule.AddAntecedent(new IsClause("change_weight", "Y"));
            rule.AddAntecedent(new IsClause("fatigue", "Y"));
            rule.AddAntecedent(new IsClause("localized_pain", "Y"));
            rule.AddAntecedent(new IsClause("changes_urination", "Y"));
            rule.AddAntecedent(new IsClause("pain_difficulty_swallowing", "Y"));
            rule.setConsequent(new IsClause("category", "Cancer"));
            rie.AddRule(rule);

            return rie;

        }

        /// <summary>
        /// Main, se crea una instancia del motor de inferencia de reglas RuleInferenceEngine llamando al método GetInferenceEngine.
        /// Luego se utiliza un bucle while para realizar la inferencia hasta que se obtenga una conclusión válida o no queden condiciones por probar. 
        /// Durante cada iteración del bucle, se solicita al usuario ingresar el valor de una condición no probada y se agrega como un hecho al motor 
        /// de inferencia.
        /// Después de finalizar el bucle, se muestra la conclusión obtenida y el estado de la memoria del motor de inferencia, en caso de no hayar una
        /// conclusion, se realiza un llamado con los sintomas almacenados a la funcion TheoremBayes que como su nombre indica, con los sintomas obtenidos
        /// y las probabilidades previamente almacenadas se calcula mediante el teorema de bayes la probabilidad de tener una enfermedad dependiendo
        /// de la cantidad de sintomas comunes que tenga con respecto al resto
        /// </summary>
        static void Main(string[] args)
        {

            RuleInferenceEngine rie = getInferenceEngine();

            Console.WriteLine("Infer with All Facts Cleared:");
            rie.ClearFacts();

            List<Clause> unproved_conditions = new List<Clause>();
            List<string> tmp_sympthoms = new List<string>();

            Clause conclusion = null;

            while (true)
            {
                conclusion = rie.Infer("sub_disease", unproved_conditions);
                if (conclusion is null || conclusion.Variable != "sub_disease")
                {
                    if (!unproved_conditions.Any())
                    {
                        var final = TheoremBayes(CreateDDBB(), tmp_sympthoms);
                        Console.WriteLine("Segun el sistema experto probablemente usted tenga o padezca : " + final.nameillness +" en un : "+final.bayes*100+"%");
                        break;
                    }
                    Clause tmp = unproved_conditions.First();
                    Console.WriteLine("¿" + tmp.Variable + "? (Y/n)");
                    unproved_conditions.Clear();
                    String value = Console.ReadLine();
                    if (value.ToUpper() is "Y") { tmp_sympthoms.Add(tmp.Variable); }
                    //else if (tmp.Variable) { }
                    rie.AddFact(new IsClause(tmp.Variable, value));
                }
                else
                {
                    conclusion = rie.Infer("sub_disease", unproved_conditions);
                    Console.WriteLine("Conclusion: " + conclusion);
                    Console.WriteLine("Memory: ");
                    Console.WriteLine("{0}", rie.Facts);
                    break;
                }
            }

            
        }
    }
}
