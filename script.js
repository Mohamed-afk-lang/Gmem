const questions = [
  {
    tag: 'FACTS',
    prompt: 'In a Knowledge Base, a FACT is best described as:',
    options: [
      'A) An instruction telling the computer what to do',
      'B) A statement that is known to be true about the world',
      'C) A rule that connects two pieces of information'
    ],
    correct: 1,
    explanation: `A FACT is a piece of information we accept as true.
Examples: 'Socrates is a man.' 'The sky is blue.'
Facts are the raw data stored in the Knowledge Base.`
  },
  {
    tag: 'RULES',
    prompt: 'A RULE in AI Knowledge Representation looks like:',
    options: [
      'A) IF <condition> THEN <conclusion>',
      'B) FOR EACH item DO something',
      'C) WHILE unknown REPEAT guess'
    ],
    correct: 0,
    explanation: `Rules express relationships between facts using IF/THEN logic.
Example: IF 'Socrates is a man' AND 'All men are mortal'
         THEN 'Socrates is mortal'.
Rules are how the system reasons beyond what it was told directly.`
  },
  {
    tag: 'KNOWLEDGE BASE',
    prompt: 'A KNOWLEDGE BASE is:',
    options: [
      'A) A programming language for writing AI code',
      'B) A database that only stores numbers',
      'C) A structured store of facts and rules about a domain'
    ],
    correct: 2,
    explanation: `The Knowledge Base (KB) is the AI's 'memory'.
It holds FACTS ('birds have wings') and
RULES ('IF has wings AND can fly THEN is a bird').
Expert systems use a KB to encode human expertise.`
  },
  {
    tag: 'INFERENCE ENGINE',
    prompt: 'The INFERENCE ENGINE is responsible for:',
    options: [
      'A) Storing new facts entered by the user',
      'B) Applying rules to known facts to derive new conclusions',
      'C) Drawing the user interface of the AI system'
    ],
    correct: 1,
    explanation: `The Inference Engine is the 'brain' that reasons.
It takes the KB (facts + rules) and fires rules to
derive facts the system was never told directly.
Two common strategies: Forward Chaining & Backward Chaining.`
  },
  {
    tag: 'INFERENCE ENGINE',
    prompt: 'FORWARD CHAINING starts from ___ and works toward ___.',
    options: [
      'A) The goal → known facts',
      'B) Known facts → the goal',
      'C) Random guesses → certainty'
    ],
    correct: 1,
    explanation: `Forward Chaining is 'data-driven':
Start with what you KNOW, apply rules, derive new facts,
and keep going until you reach the GOAL (or get stuck).
Backward Chaining is the opposite: start from the goal
and ask 'what facts would prove this?'`
  },
  {
    tag: 'LOGIC',
    prompt: 'Which statement uses FIRST-ORDER LOGIC (not propositional logic)?',
    options: [
      'A) "It is raining AND the ground is wet"',
      'B) "FOR ALL x: IF Man(x) THEN Mortal(x)"',
      'C) "The light is on OR the light is off"'
    ],
    correct: 1,
    explanation: `First-Order Logic (FOL) adds VARIABLES and QUANTIFIERS
(FOR ALL, THERE EXISTS) to propositional logic.
This makes it far more expressive — you can write
general rules that apply to entire categories of objects,
not just individual named things.`
  },
  {
    tag: 'UNCERTAINTY',
    prompt: 'When an AI cannot be 100% certain of a fact, it can use:',
    options: [
      'A) Random number generation to decide',
      'B) Probability or certainty factors to represent confidence',
      'C) A larger screen to display the confusion'
    ],
    correct: 1,
    explanation: `Real-world knowledge is rarely black-and-white.
Systems like MYCIN used CERTAINTY FACTORS (0..1).
Modern AI uses PROBABILITY THEORY and BAYESIAN NETWORKS
to reason confidently even with incomplete information.`
  },
  {
    tag: 'ONTOLOGY',
    prompt: 'An ONTOLOGY in AI is:',
    options: [
      'A) A branch of philosophy with no use in computing',
      'B) A formal specification of concepts and their relationships in a domain',
      'C) The physical hardware that runs the inference engine'
    ],
    correct: 1,
    explanation: `An Ontology defines the 'vocabulary' of a domain:
what types of things exist, their properties, and
how they relate to each other.
Example: a medical ontology defines 'Disease', 'Symptom',
'Treatment', and the relationships between them.
Web technologies like OWL and RDF are built on this idea.`
  }
];

const screen = document.getElementById('screen');
const maxLives = 3;
const questionsPerLevel = 4;
const totalLevels = Math.ceil(questions.length / questionsPerLevel);
let score = 0;
let currentIndex = 0;
let lives = maxLives;
let streak = 0;

function getLevel() {
  return Math.floor(currentIndex / questionsPerLevel) + 1;
}

function getSummaryText(scoreValue) {
  const pct = Math.round((scoreValue / questions.length) * 100);
  if (pct === 100) {
    return '🌟 Perfect score! You are a true Knowledge Engineer.';
  }
  if (pct >= 75) {
    return '🎓 Great job! You have a solid grasp of knowledge representation.';
  }
  if (pct >= 50) {
    return '📚 Not bad! Review the lessons and try again.';
  }
  return '🔍 Keep studying — every expert started as a beginner!';
}

function renderWelcome() {
  screen.innerHTML = `
    <div class="card animate-card">
      <div class="status-chip">Start Screen</div>
      <h2>AI Knowledge Quest</h2>
      <p class="details">A knowledge-based quiz with levels, lives, and streak bonuses. Stay sharp and keep your streak alive.</p>
      <div class="button-row">
        <button class="primary" onclick="startGame()">Start Game</button>
        <button class="secondary" onclick="renderInstructions()">How to Play</button>
      </div>
    </div>
  `;
}

function renderInstructions() {
  screen.innerHTML = `
    <div class="card animate-card">
      <div class="status-chip">How to Play</div>
      <h2>Game Rules</h2>
      <p class="details">Answer questions correctly to keep your lives and build a streak.</p>
      <div class="review-list">
        <div><strong>Lives:</strong> You start with ${maxLives}. Lose one for each wrong answer.</div>
        <div><strong>Streak:</strong> Correct answers in a row increase your streak.</div>
        <div><strong>Levels:</strong> There are ${totalLevels} levels. Complete each one to progress.</div>
        <div><strong>Final demo:</strong> Finish the quiz to see a live inference demo.</div>
      </div>
      <div class="button-row">
        <button class="primary" onclick="startGame()">Play Now</button>
        <button class="secondary" onclick="renderWelcome()">Back</button>
      </div>
    </div>
  `;
}

function renderQuestion() {
  const question = questions[currentIndex];
  const progress = Math.round((currentIndex / questions.length) * 100);

  screen.innerHTML = `
    <div class="card animate-card">
      <div class="status-row">
        <span class="status-chip">Level ${getLevel()} / ${totalLevels}</span>
        <span class="status-chip">Lives ${'❤'.repeat(lives)}${'♡'.repeat(maxLives - lives)}</span>
        <span class="status-chip">Streak ${streak}</span>
      </div>
      <div class="progress"><div class="fill" style="width: ${progress}%"></div></div>
      <h2>${question.prompt}</h2>
      <p class="details">Concept: ${question.tag}</p>
      <div class="option-list" id="options"></div>
    </div>
    <div class="button-row">
      <button class="secondary" onclick="renderWelcome()">Restart</button>
    </div>
  `;

  const optionsEl = document.getElementById('options');
  question.options.forEach((text, optionIndex) => {
    const button = document.createElement('button');
    button.className = 'option-button';
    button.textContent = text;
    button.onclick = () => selectOption(optionIndex);
    optionsEl.appendChild(button);
  });
}

function selectOption(optionIndex) {
  const question = questions[currentIndex];
  const buttons = document.querySelectorAll('.option-button');
  buttons.forEach((button, index) => {
    button.disabled = true;
    if (index === question.correct) {
      button.classList.add('correct');
    }
    if (index === optionIndex && index !== question.correct) {
      button.classList.add('wrong');
    }
  });

  const isCorrect = optionIndex === question.correct;
  if (isCorrect) {
    score += 1;
    streak += 1;
  } else {
    lives -= 1;
    streak = 0;
  }

  showQuestionFeedback(isCorrect);
}

function showQuestionFeedback(isCorrect) {
  const question = questions[currentIndex];
  const resultText = isCorrect
    ? '✅ Correct! Keep the streak going.'
    : `❌ Not quite. The correct answer was ${String.fromCharCode(65 + question.correct)}.`;

  const card = document.createElement('div');
  card.className = 'card response animate-card';
  const buttonText = lives <= 0 ? 'Game Over' : currentIndex + 1 === questions.length ? 'Show Inference Demo' : 'Next Question';

  card.innerHTML = `
    <strong>${resultText}</strong>
    <p>${question.explanation.replace(/\n/g, '<br>')}</p>
    <div class="button-row">
      <button class="primary" onclick="nextStep()">${buttonText}</button>
    </div>
  `;
  screen.appendChild(card);
}

function nextStep() {
  if (lives <= 0) {
    renderGameOver();
    return;
  }

  currentIndex += 1;

  if (currentIndex >= questions.length) {
    renderInferenceDemo();
    return;
  }

  if (currentIndex % questionsPerLevel === 0) {
    renderLevelComplete();
    return;
  }

  renderQuestion();
}

function renderLevelComplete() {
  const levelNumber = getLevel() - 1;
  screen.innerHTML = `
    <div class="card animate-card">
      <div class="status-chip">Level Complete</div>
      <h2>You finished Level ${levelNumber}!</h2>
      <p class="details">Keep your lives and streak to unlock the next challenge.</p>
      <div class="response">
        <strong>Score: ${score} / ${questions.length}</strong>
        <p>Current streak: ${streak}</p>
      </div>
      <div class="button-row">
        <button class="primary" onclick="renderQuestion()">Continue to Level ${getLevel()}</button>
      </div>
    </div>
  `;
}

function renderInferenceDemo() {
  screen.innerHTML = `
    <div class="card animate-card">
      <div class="status-chip">Live Inference Demo</div>
      <h2>Watch the Knowledge Base reason</h2>
      <p class="details">This demo loads facts and rules, then derives a new fact automatically.</p>
      <div class="review-list">
        <div><strong>Step 1:</strong> Add fact: 'Socrates is a man'</div>
        <div><strong>Step 2:</strong> Add fact: 'All men are mortal'</div>
        <div><strong>Step 3:</strong> Add rule: IF 'All men are mortal' THEN 'Socrates is mortal'</div>
        <div><strong>Result:</strong> The engine derives <em>'Socrates is mortal'</em> from the facts and rule.</div>
      </div>
    </div>
    <div class="card response animate-card">
      <strong>Your score: ${score} / ${questions.length}</strong>
      <p>${getSummaryText(score)}</p>
      <div class="button-row">
        <button class="primary" onclick="renderFinalScore()">See Final Score</button>
      </div>
    </div>
  `;
}

function renderFinalScore() {
  const pct = Math.round((score / questions.length) * 100);
  screen.innerHTML = `
    <div class="card animate-card">
      <div class="status-chip">Game Over</div>
      <h2>Final Score</h2>
      <p class="details">You answered ${score} out of ${questions.length} questions correctly.</p>
      <div class="response">
        <strong>${getSummaryText(score)}</strong>
        <p>Remaining lives: ${lives}</p>
        <p>Top streak: ${streak}</p>
      </div>
      <div class="button-row">
        <button class="primary" onclick="restart()">Play Again</button>
      </div>
    </div>
  `;
}

function renderGameOver() {
  screen.innerHTML = `
    <div class="card animate-card">
      <div class="status-chip">Game Over</div>
      <h2>You ran out of lives.</h2>
      <p class="details">Try again to beat the quiz with a higher streak.</p>
      <div class="response">
        <strong>Score: ${score} / ${questions.length}</strong>
        <p>Best streak: ${streak}</p>
      </div>
      <div class="button-row">
        <button class="primary" onclick="restart()">Try Again</button>
      </div>
    </div>
  `;
}

function startGame() {
  score = 0;
  currentIndex = 0;
  lives = maxLives;
  streak = 0;
  renderQuestion();
}

function restart() {
  renderWelcome();
}

renderWelcome();
