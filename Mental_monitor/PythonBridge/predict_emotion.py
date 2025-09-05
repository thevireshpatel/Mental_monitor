#!/usr/bin/env python3
"""
predict_emotion.py – transformer pipeline (GoEmotions 28-class)
Reads one line from stdin, returns JSON dict emotion->probability.
"""
import json
import sys
from pathlib import Path
import torch
from transformers import AutoTokenizer, AutoModelForSequenceClassification

# ---------- config ----------
MODEL_DIR = Path(__file__).parent.resolve()
DEVICE = "cuda" if torch.cuda.is_available() else "cpu"

# GoEmotions class order (28 labels)
GO_EMOTIONS = [
    "admiration", "amusement", "anger", "annoyance", "approval", "caring",
    "confusion", "curiosity", "desire", "disappointment", "disapproval",
    "disgust", "embarrassment", "excitement", "fear", "gratitude", "grief",
    "joy", "love", "nervousness", "optimism", "pride", "realization",
    "relief", "remorse", "sadness", "surprise", "neutral"
]

# ---------- load once ----------
tokenizer = AutoTokenizer.from_pretrained(MODEL_DIR)
model = AutoModelForSequenceClassification.from_pretrained(MODEL_DIR).to(DEVICE)
num_labels = len(model.config.id2label)
assert num_labels == len(GO_EMOTIONS), f"Model has {num_labels} classes, expected {len(GO_EMOTIONS)}"

# ---------- predict ----------
def predict(text: str) -> dict:
    inputs = tokenizer(
        text,
        return_tensors="pt",
        truncation=True,
        padding=True,
        max_length=512
    ).to(DEVICE)

    with torch.no_grad():
        logits = model(**inputs).logits[0]
    probs = torch.softmax(logits, dim=-1).cpu().tolist()

    return {GO_EMOTIONS[i]: round(float(p), 4) for i, p in enumerate(probs)}


# ---------- main ----------
if __name__ == "__main__":
    line = sys.stdin.readline().strip()   # single-line input
    print(json.dumps(predict(line)), flush=True)