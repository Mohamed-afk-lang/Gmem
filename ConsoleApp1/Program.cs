using System;
using System.Collections.Generic;
 
namespace AIKnowledgeGame
{
    // ── Data model for one question ──────────────────────────────────────────
    class Question
    {
        public string ConceptTag   { get; set; }  // e.g. "FACT", "RULE"
        public string Prompt       { get; set; }  // The question text
        public string[] Options    { get; set; }  // A, B, C choices
        public int    CorrectIndex { get; set; }  // 0-based index of right answer
        public string Explanation  { get; set; }  // Teaching note shown after answer
    }
 
    // ── Simple simulated Knowledge Base used in the inference demo ───────────
    class KnowledgeBase
    {
        // Facts: things we know are true
        private readonly List<string> _facts = new List<string>();
 
        // Rules: IF antecedent THEN consequent
        private readonly List<(string antecedent, string consequent)> _rules
            = new List<(string, string)>();
 
        public void AddFact(string fact)           => _facts.Add(fact.ToLower());
        public void AddRule(string if_, string then_) => _rules.Add((if_.ToLower(), then_.ToLower()));
        public bool HasFact(string fact)           => _facts.Contains(fact.ToLower());
 
        // ── Inference Engine: forward chaining ───────────────────────────────
        // Keep firing rules whose antecedent is known until nothing new is found.
        public List<string> Infer()
        {
            var derived = new List<string>();
            bool changed = true;
 
            while (changed)
            {
                changed = false;
                foreach (var rule in _rules)
                {
                    // If we know the antecedent AND haven't derived the consequent yet…
                    if (HasFact(rule.antecedent) && !HasFact(rule.consequent))
                    {
                        _facts.Add(rule.consequent);   // assert the new fact
                        derived.Add(rule.consequent);  // record what we derived
                        changed = true;
                    }
                }
            }
            return derived;
        }
    }
 
    // ── Main game class ──────────────────────────────────────────────────────
    class Game
    {
        // Colour helpers (gracefully ignored on terminals that don't support it)
        static void Print(string text, ConsoleColor color = ConsoleColor.White)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
 
        static void Pause()
        {
            Console.WriteLine();
            Print("  Press ENTER to continue...", ConsoleColor.DarkGray);
            Console.ReadLine();
            Console.Clear();
        }
 
        // ── Question bank (8 questions) ──────────────────────────────────────
        static List<Question> BuildQuestions()
        {
            return new List<Question>
            {
                // ── 1. Facts ──────────────────────────────────────────────────
                new Question
                {
                    ConceptTag   = "FACTS",
                    Prompt       = "In a Knowledge Base, a FACT is best described as:",
                    Options      = new[]
                    {
                        "A) An instruction telling the computer what to do",
                        "B) A statement that is known to be true about the world",
                        "C) A rule that connects two pieces of information"
                    },
                    CorrectIndex = 1,
                    Explanation  =
                        "A FACT is a piece of information we accept as true.\n" +
                        "  Examples: 'Socrates is a man.'  'The sky is blue.'\n" +
                        "  Facts are the raw data stored in the Knowledge Base."
                },
 
                // ── 2. Rules ─────────────────────────────────────────────────
                new Question
                {
                    ConceptTag   = "RULES",
                    Prompt       = "A RULE in AI Knowledge Representation looks like:",
                    Options      = new[]
                    {
                        "A) IF <condition> THEN <conclusion>",
                        "B) FOR EACH item DO something",
                        "C) WHILE unknown REPEAT guess"
                    },
                    CorrectIndex = 0,
                    Explanation  =
                        "Rules express relationships between facts using IF/THEN logic.\n" +
                        "  Example: IF 'Socrates is a man' AND 'All men are mortal'\n" +
                        "           THEN 'Socrates is mortal'.\n" +
                        "  Rules are how the system reasons beyond what it was told directly."
                },
 
                // ── 3. Knowledge Base ─────────────────────────────────────────
                new Question
                {
                    ConceptTag   = "KNOWLEDGE BASE",
                    Prompt       = "A KNOWLEDGE BASE is:",
                    Options      = new[]
                    {
                        "A) A programming language for writing AI code",
                        "B) A database that only stores numbers",
                        "C) A structured store of facts and rules about a domain"
                    },
                    CorrectIndex = 2,
                    Explanation  =
                        "The Knowledge Base (KB) is the AI's 'memory'.\n" +
                        "  It holds FACTS ('birds have wings') and\n" +
                        "  RULES ('IF has wings AND can fly THEN is a bird').\n" +
                        "  Expert systems use a KB to encode human expertise."
                },
 
                // ── 4. Inference Engine ───────────────────────────────────────
                new Question
                {
                    ConceptTag   = "INFERENCE ENGINE",
                    Prompt       = "The INFERENCE ENGINE is responsible for:",
                    Options      = new[]
                    {
                        "A) Storing new facts entered by the user",
                        "B) Applying rules to known facts to derive new conclusions",
                        "C) Drawing the user interface of the AI system"
                    },
                    CorrectIndex = 1,
                    Explanation  =
                        "The Inference Engine is the 'brain' that reasons.\n" +
                        "  It takes the KB (facts + rules) and fires rules to\n" +
                        "  derive facts the system was never told directly.\n" +
                        "  Two common strategies: Forward Chaining & Backward Chaining."
                },
 
                // ── 5. Forward vs Backward chaining ──────────────────────────
                new Question
                {
                    ConceptTag   = "INFERENCE ENGINE",
                    Prompt       = "FORWARD CHAINING starts from ___ and works toward ___.",
                    Options      = new[]
                    {
                        "A) The goal → known facts",
                        "B) Known facts → the goal",
                        "C) Random guesses → certainty"
                    },
                    CorrectIndex = 1,
                    Explanation  =
                        "Forward Chaining is 'data-driven':\n" +
                        "  Start with what you KNOW, apply rules, derive new facts,\n" +
                        "  and keep going until you reach the GOAL (or get stuck).\n" +
                        "  Backward Chaining is the opposite: start from the goal\n" +
                        "  and ask 'what facts would prove this?'"
                },
 
                // ── 6. Propositional vs First-Order Logic ─────────────────────
                new Question
                {
                    ConceptTag   = "LOGIC",
                    Prompt       = "Which statement uses FIRST-ORDER LOGIC (not propositional logic)?",
                    Options      = new[]
                    {
                        "A) 'It is raining AND the ground is wet'",
                        "B) 'FOR ALL x: IF Man(x) THEN Mortal(x)'",
                        "C) 'The light is on OR the light is off'"
                    },
                    CorrectIndex = 1,
                    Explanation  =
                        "First-Order Logic (FOL) adds VARIABLES and QUANTIFIERS\n" +
                        "  (FOR ALL, THERE EXISTS) to propositional logic.\n" +
                        "  This makes it far more expressive — you can write\n" +
                        "  general rules that apply to entire categories of objects,\n" +
                        "  not just individual named things."
                },
 
                // ── 7. Uncertainty ────────────────────────────────────────────
                new Question
                {
                    ConceptTag   = "UNCERTAINTY",
                    Prompt       = "When an AI cannot be 100% certain of a fact, it can use:",
                    Options      = new[]
                    {
                        "A) Random number generation to decide",
                        "B) Probability or certainty factors to represent confidence",
                        "C) A larger screen to display the confusion"
                    },
                    CorrectIndex = 1,
                    Explanation  =
                        "Real-world knowledge is rarely black-and-white.\n" +
                        "  Systems like MYCIN used CERTAINTY FACTORS (0..1).\n" +
                        "  Modern AI uses PROBABILITY THEORY and BAYESIAN NETWORKS\n" +
                        "  to reason confidently even with incomplete information."
                },
 
                // ── 8. Ontology ───────────────────────────────────────────────
                new Question
                {
                    ConceptTag   = "ONTOLOGY",
                    Prompt       = "An ONTOLOGY in AI is:",
                    Options      = new[]
                    {
                        "A) A branch of philosophy with no use in computing",
                        "B) A formal specification of concepts and their relationships in a domain",
                        "C) The physical hardware that runs the inference engine"
                    },
                    CorrectIndex = 1,
                    Explanation  =
                        "An Ontology defines the 'vocabulary' of a domain:\n" +
                        "  what types of things exist, their properties, and\n" +
                        "  how they relate to each other.\n" +
                        "  Example: a medical ontology defines 'Disease', 'Symptom',\n" +
                        "  'Treatment', and the relationships between them.\n" +
                        "  Web technologies like OWL and RDF are built on this idea."
                }
            };
        }
 
        // ── Render the welcome banner ─────────────────────────────────────────
        static void ShowWelcome()
        {
            Console.Clear();
            Print("╔══════════════════════════════════════════════════════════╗", ConsoleColor.Cyan);
            Print("║    AI KNOWLEDGE QUEST — Knowledge Representation Game    ║", ConsoleColor.Cyan);
            Print("╚══════════════════════════════════════════════════════════╝", ConsoleColor.Cyan);
            Console.WriteLine();
            Print("  Welcome, Knowledge Engineer!", ConsoleColor.Yellow);
            Console.WriteLine();
            Console.WriteLine("  In this game you will explore the building blocks that");
            Console.WriteLine("  let AI systems KNOW things and REASON about them:");
            Console.WriteLine();
            Print("   🧩  Facts          — what the system knows", ConsoleColor.White);
            Print("   📐  Rules          — IF...THEN logic", ConsoleColor.White);
            Print("   🗄️  Knowledge Base  — the AI's memory", ConsoleColor.White);
            Print("   ⚙️  Inference       — reasoning to new conclusions", ConsoleColor.White);
            Print("   🌫️  Uncertainty     — handling incomplete knowledge", ConsoleColor.White);
            Console.WriteLine();
            Console.WriteLine("  Answer each question (type A, B, or C).");
            Console.WriteLine("  You'll get instant feedback and a short lesson after each one.");
            Console.WriteLine();
            Print("  Ready? Let's begin!", ConsoleColor.Green);
            Pause();
        }
 
        // ── Ask one question and return true if answered correctly ────────────
        static bool AskQuestion(Question q, int number, int total)
        {
            // ── Header ────────────────────────────────────────────────────────
            Print($"  Question {number} of {total}  ·  Concept: [{q.ConceptTag}]",
                  ConsoleColor.DarkCyan);
            Console.WriteLine(new string('─', 58));
            Console.WriteLine();
            Print($"  {q.Prompt}", ConsoleColor.White);
            Console.WriteLine();
 
            // ── Options ─────────────────────────────────────────────────────
            foreach (var opt in q.Options)
            {
                Print($"    {opt}", ConsoleColor.Gray);
            }
            Console.WriteLine();
 
            // ── Read and validate input ─────────────────────────────────────
            char answer = ' ';
            while (answer != 'A' && answer != 'B' && answer != 'C')
            {
                Print("  Your answer (A / B / C): ", ConsoleColor.Yellow);
                Console.Write("  > ");
                string raw = Console.ReadLine()?.Trim().ToUpper() ?? "";
                if (raw.Length > 0) answer = raw[0];
 
                if (answer != 'A' && answer != 'B' && answer != 'C')
                    Print("  ⚠  Please type A, B, or C.", ConsoleColor.Red);
            }
 
            // ── Evaluate ──────────────────────────────────────────────────────
            int answerIndex = answer - 'A';          // 'A'→0, 'B'→1, 'C'→2
            bool correct    = answerIndex == q.CorrectIndex;
            char correctLetter = (char)('A' + q.CorrectIndex);
 
            Console.WriteLine();
            if (correct)
            {
                Print("  ✅  CORRECT! Well done.", ConsoleColor.Green);
            }
            else
            {
                Print($"  ❌  Not quite. The correct answer was {correctLetter}.", ConsoleColor.Red);
            }
 
            // ── Mini lesson ───────────────────────────────────────────────────
            Console.WriteLine();
            Print("  📖  Quick Lesson:", ConsoleColor.Cyan);
            foreach (var line in q.Explanation.Split('\n'))
                Print("  " + line, ConsoleColor.DarkGray);
 
            Pause();
            return correct;
        }
 
        // ── Live inference demo ─────────────────────────────────────────────
        // Shows the player a real mini Knowledge Base + Inference Engine in action.
        static void RunInferenceDemo()
        {
            Console.Clear();
            Print("╔══════════════════════════════════════════════════════════╗", ConsoleColor.Magenta);
            Print("║            🔬  LIVE INFERENCE ENGINE DEMO                ║", ConsoleColor.Magenta);
            Print("╚══════════════════════════════════════════════════════════╝", ConsoleColor.Magenta);
            Console.WriteLine();
            Print("  Watch a tiny Knowledge Base reason for itself!", ConsoleColor.Yellow);
            Console.WriteLine();
 
            // Build the KB
            var kb = new KnowledgeBase();
 
            Print("  Step 1 — Loading FACTS into the Knowledge Base...", ConsoleColor.Cyan);
            kb.AddFact("socrates is a man");
            Print("    ✔  FACT added: 'Socrates is a man'",        ConsoleColor.DarkGray);
            kb.AddFact("all men are mortal");
            Print("    ✔  FACT added: 'All men are mortal'",       ConsoleColor.DarkGray);
            System.Threading.Thread.Sleep(800);
 
            Console.WriteLine();
            Print("  Step 2 — Loading RULES into the Knowledge Base...", ConsoleColor.Cyan);
            kb.AddRule("all men are mortal", "socrates is mortal");
            Print("    ✔  RULE added: IF 'All men are mortal'", ConsoleColor.DarkGray);
            Print("                   THEN 'Socrates is mortal'", ConsoleColor.DarkGray);
            System.Threading.Thread.Sleep(800);
 
            Console.WriteLine();
            Print("  Step 3 — Running the INFERENCE ENGINE (Forward Chaining)...", ConsoleColor.Cyan);
            System.Threading.Thread.Sleep(800);
            var derived = kb.Infer();
 
            Console.WriteLine();
            if (derived.Count > 0)
            {
                Print("  ⚡  New facts DERIVED automatically:", ConsoleColor.Green);
                foreach (var f in derived)
                    Print($"      → '{f}'", ConsoleColor.Green);
            }
            else
            {
                Print("  (No new facts could be derived.)", ConsoleColor.DarkGray);
            }
 
            Console.WriteLine();
            Print("  This is exactly how EXPERT SYSTEMS like MYCIN worked —", ConsoleColor.White);
            Print("  they stored medical rules and inferred diagnoses from symptoms.", ConsoleColor.White);
 
            Pause();
        }
 
        // ── Final score screen ──────────────────────────────────────────────
        static void ShowFinalScore(int score, int total)
        {
            Console.Clear();
            Print("╔══════════════════════════════════════════════════════════╗", ConsoleColor.Cyan);
            Print("║                    🏆  GAME OVER                        ║", ConsoleColor.Cyan);
            Print("╚══════════════════════════════════════════════════════════╝", ConsoleColor.Cyan);
            Console.WriteLine();
 
            double pct = (double)score / total * 100;
 
            Print($"  Your final score:  {score} / {total}  ({pct:F0}%)", ConsoleColor.Yellow);
            Console.WriteLine();
 
            // Personalised message based on performance
            if (pct == 100)
            {
                Print("  🌟  PERFECT SCORE! You are a true Knowledge Engineer!", ConsoleColor.Green);
            }
            else if (pct >= 75)
            {
                Print("  🎓  Great job! You have a solid grasp of KR in AI.", ConsoleColor.Green);
            }
            else if (pct >= 50)
            {
                Print("  📚  Not bad! Review the lessons above and try again.", ConsoleColor.Yellow);
            }
            else
            {
                Print("  🔍  Keep studying — every expert started as a beginner!", ConsoleColor.Red);
            }
 
            Console.WriteLine();
            Print("  Concepts covered in this game:", ConsoleColor.Cyan);
            Print("    • Facts & Rules              — the atoms of knowledge",     ConsoleColor.DarkGray);
            Print("    • Knowledge Base             — structured storage of KR",   ConsoleColor.DarkGray);
            Print("    • Inference Engine           — automated reasoning",        ConsoleColor.DarkGray);
            Print("    • Forward / Backward Chain.  — two reasoning strategies",   ConsoleColor.DarkGray);
            Print("    • First-Order Logic          — expressive representation",  ConsoleColor.DarkGray);
            Print("    • Uncertainty                — probability & confidence", ConsoleColor.DarkGray);
            Print("    • Ontology                   — shared domain vocabulary",   ConsoleColor.DarkGray);
            Console.WriteLine();
            Print("  Thanks for playing — keep building smarter systems! 🤖", ConsoleColor.Cyan);
            Console.WriteLine();
        }
 
        // ── Entry point ───────────────────────────────────────────────────────
        static void Main(string[] args)
        {
            // Allow emoji/unicode in the console (works on modern terminals)
            Console.OutputEncoding = System.Text.Encoding.UTF8;
 
            ShowWelcome();
 
            var questions = BuildQuestions();
            int score = 0;
 
            // ── Quiz loop ─────────────────────────────────────────────────────
            for (int i = 0; i < questions.Count; i++)
            {
                Console.Clear();
 
                // Show a mini score bar at the top of each question
                Print($"  Score so far: {score}/{i}  " +
                      new string('█', score) + new string('░', i - score),
                      ConsoleColor.DarkCyan);
                Console.WriteLine();
 
                bool correct = AskQuestion(questions[i], i + 1, questions.Count);
                if (correct) score++;
            }
 
            // ── Live demo after the quiz ──────────────────────────────────────────
            RunInferenceDemo();
 
            // ── Final results ─────────────────────────────────────────────────
            ShowFinalScore(score, questions.Count);
 
            Print("  Press ENTER to exit.", ConsoleColor.DarkGray);
            Console.ReadLine();
        }
    }
}

