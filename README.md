# Mental Monitor  
**Your Emotional Wellness Companion**  

Track moods, spot patterns, and grow stronger—one journal entry at a time.

## Abstract  
Mental Monitor is an ASP.NET Core MVC web application that helps users track emotional well-being through AI-powered journal analysis. Entries are classified into 28 GoEmotions categories, visualised over time, and used to trigger gentle alerts when negative patterns emerge. The entire pipeline runs locally (Python transformer + SQLite) and ships as a single, self-contained EXE.

## Key Features  
- 🖊️ **Express** – write freely; AI uncovers emotions in seconds  
- 📊 **Understand** – interactive charts, trends, CSV export  
- 🚨 **Grow** – smart alerts when sadness/anger/fear exceed 25 %  
- 🌓 **Adapt** – emotion-responsive UI (colors, glow, animations)  
- 🔐 **Private** – SQLite file, no cloud, anonymous mode toggle  
- 📱 **Mobile** – swipe cards, bottom nav, touch gestures  

## Tech Stack  
- **Back-End**: ASP.NET Core 9, Entity Framework Core 9, SQLite  
- **AI**: Python 3.10+, Hugging Face Transformers, Google GoEmotions  
- **Front-End**: Bootstrap 5, Chart.js, AOS, custom CSS/JS  
- **ML Model**: Distil-BERT fine-tuned on 28-class GoEmotions dataset  

## Quick Start  
```bash
# 1. Clone repo
[https://github.com/thevireshpatel/Mental_monitor.git]
cd Mental_monitor

# 2. Install Python deps
python -m venv .venv
.venv\Scripts\activate
pip install -r PythonBridge/requirements.txt

# 3. Run once
dotnet ef database update
dotnet run
