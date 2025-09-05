"""
Minimal training script using GoEmotions TSV (google-research/goemotions).
Produces emotion_model.pkl & vectorizer.pkl for predict_emotion.py
"""
import pandas as pd
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.linear_model import LogisticRegression
from sklearn.preprocessing import MultiLabelBinarizer
from sklearn.model_selection import train_test_split
import joblib, ast, pathlib, itertools

DATA = pathlib.Path(__file__).with_name("journal_data.csv")
MODEL = pathlib.Path(__file__).with_name("emotion_model.pkl")
VEC   = pathlib.Path(__file__).with_name("vectorizer.pkl")

def main():
    df = pd.read_csv(DATA)
    # GoEmotions stores labels as string list: "['admiration','joy']"
    df["labels"] = df["emotions"].apply(ast.literal_eval)
    texts = df["text"].astype(str).tolist()
    mlb = MultiLabelBinarizer()
    y = mlb.fit_transform(df["labels"])

    tv = TfidfVectorizer(max_features=20_000, ngram_range=(1,2))
    X = tv.fit_transform(texts)

    clf = LogisticRegression(max_iter=1000)
    clf.fit(X, y)

    joblib.dump({"model": clf, "classes": mlb.classes_}, MODEL)
    joblib.dump(tv, VEC)
    print("Saved", MODEL, VEC)

if __name__ == "__main__":
    main()