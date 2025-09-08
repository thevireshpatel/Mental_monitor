# Mental Monitor  
**Your Emotional Wellness Companion**  

Track moods, spot patterns, and grow stronger—one journal entry at a time.

---

## 🧠 Introduction  

Mental health disorders affect over **970 million** people worldwide (WHO, 2023).  
Traditional journaling lacks **real-time feedback**, leaving users without actionable insights.  

**Hypothesis**: If users receive **immediate, visual feedback** about their emotional state—delivered through an **adaptive, privacy-first** interface—they will journal more consistently and gain better emotional awareness.  

**Target Audience**:  
- Young adults (18-35) seeking **private**, tech-enabled wellness tools  
- Developers who want to **extend or audit** an open-source pipeline  

**Use Case (30 s)**:  
1. Write: *“I feel completely hollow today.”*  
2. AI detects **30 % sadness** → background turns soft blue, card pulses gently  
3. Alert suggests **1-minute breathing** → user taps, completes exercise  
4. Chart shows **sadness spike** last week → exports CSV for therapist  

---

## ⚙️ Methodology  

### System Architecture  
Browser  →  ASP.NET Core  →  PythonBridge  →  SQLite
Bootstrap     Controllers      predict_emotion     Entries/Scores/Alerts
Chart.js      Services         transformers        Identity
Copy

### Emotion Detection Pipeline  
1. **Input**: raw journal text (≤ 2000 chars)  
2. **Pre-processing**: lower-case, strip HTML  
3. **Model**: Distil-BERT fine-tuned on **Google GoEmotions** (28 classes)  
4. **Output**: JSON `{emotion: probability}`  
5. **Threshold**: > 25 % triggers **gentle alert** (user-dismissible)  

### Alert Logic  
- **Negative emotions**: sadness, anger, fear, disappointment, disgust, grief, remorse  
- **Trend rule**: last 3 entries average sadness > 25 % → **Critical**  
- **Local only**: no cloud, no push, no diagnosis  

### Tech Stack  
- **Back-End**: ASP.NET Core 9, Entity Framework Core 9, **SQLite**  
- **AI**: Python 3.10+, Hugging Face `transformers`, **local model**  
- **Front-End**: Bootstrap 5, Chart.js, AOS, **emotion-responsive CSS**  
- **Auth**: ASP.NET Core Identity (email + password hash)  
- **Deploy**: Self-contained EXE or Docker image  

---

## 🎨 Design  

### Emotion-Responsive Principles  
- **Colour**: background gradient shifts with dominant emotion  
  - Joy → yellow Sadness → blue Anger → red Fear → purple  
- **Glow**: card emits soft colour matching detected mood  
- **Pulse**: subtle scale animation on high-intensity emotions  

### Light / Dark Mode  
- CSS custom properties (`--clr-bg`, `--clr-text`)  
- `data-theme` attribute toggled by JS + `localStorage`  
- Images: `hero-light.jpg` / `hero-dark.jpg`, etc.  

### Micro-interactions  
- **3-D tilt** on cards (mouse-move parallax)  
- **Ripple** buttons, **confetti** on milestones  
- **Floating label** journal form  

### Accessibility  
- WCAG 2.1 AA colour contrast tested  
- Keyboard-navigable forms  
- `prefers-reduced-motion` respected  

---

## 🔮 Future Work  

### Planned Features  
- **Mobile app** – MAUI or Flutter wrapper  
- **Voice journaling** – Whisper + emotion on audio  
- **AI prompts** – locally-generated journaling questions (Llama.cpp)  
- **Push notifications** – optional Azure Notification Hubs  

### Personalisation  
- User-tunable alert threshold  
- Custom emotion labels (e.g., “exam-stress”)  
- Mood-based **playlists** & **ambient soundscapes**  

### Gamification  
- Mood streaks, badges, leaderboards (local only)  
- Emotional goals with progress bars  
- Confetti bursts on milestones  

### Research Directions  
- **Sentiment forecasting** – LSTM on user history  
- **Wearable integration** – HR, sleep, step data  
- **Federated learning** – no central server  

---
## 🚀 Quick Start  
```bash
[git clone https://github.com/thevireshpatel/Mental_monitor.git]
cd Mental_monitor
python -m venv .venv && .venv\Scripts\activate
pip install -r PythonBridge/requirements.txt
dotnet ef database update
dotnet run
Open https://localhost:7200 → Register → Journal → Trends → Alerts.
📄 License
MIT – see LICENSE.md
📚 Citation
If you use this project in research, please cite:
Patel, V. (2025). Mental Monitor: AI-Powered Emotional Wellness Tracking [Software].{https://github.com/thevireshpatel/Mental_monitor.git}
