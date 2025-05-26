from fastapi import FastAPI
from contextlib import asynccontextmanager
from models import BicycleBrand, Game, GameGenre
from dataclasses import asdict
import polars as pl
from faker import Faker
import random
import os
import duckdb


GAMES_DATA_FILE = "data/games.parquet"
bicycle_items = []


def generate_games(n: int) -> pl.DataFrame:
    fake = Faker()
    genres = list(GameGenre)

    records = []
    for _ in range(n):
        genre = random.choice(genres)
        name = fake.sentence(nb_words=3).rstrip(".")
        price = round(random.uniform(0, 60), 2)
        game = Game(genre, name, price)
        records.append(asdict(game))

    return pl.DataFrame(records)


async def setup():
    global bicycle_items
    bicycle_items.extend(
        [
            {"brand": BicycleBrand.Canyon, "model": "Endurace"},
            {"brand": BicycleBrand.Canyon, "model": "Grail"},
            {"brand": BicycleBrand.Trek, "model": "Domane"},
            {"brand": BicycleBrand.Specialized, "model": "Allez"},
            {"brand": BicycleBrand.Giant, "model": "Defy"},
            {"brand": BicycleBrand.Cannondale, "model": "Synapse"},
        ]
    )

    if os.path.exists(GAMES_DATA_FILE):
        print("Games data file already exists, skipping generation.")
        return

    print("Generating games data...")
    os.makedirs(os.path.dirname(GAMES_DATA_FILE), exist_ok=True)
    df = generate_games(100_000)
    df.write_parquet(GAMES_DATA_FILE, compression="snappy")


async def teardown():
    global bicycle_items
    bicycle_items.clear()


@asynccontextmanager
async def lifespan(app: FastAPI):
    await setup()
    yield
    await teardown()


app = FastAPI(lifespan=lifespan)


@app.get("/")
async def root():
    return {"message": "Hello, World!"}


@app.get("/bicycles/{brand}")
async def bicycles(brand: BicycleBrand, model: str = ""):
    """
    Get a list of bicycless filtered by brand and model.
    """
    print(bicycle_items)
    filtered_bicycles = [
        bicycle
        for bicycle in bicycle_items
        if bicycle["brand"] == brand and (not model or bicycle["model"] == model)
    ]

    return {"bicycles": filtered_bicycles}


@app.get("/games/{genre}")
async def games(genre: GameGenre, title: str = ""):
    """
    Get a list of games filtered by genre and optional title using Polars.
    """
    df = pl.read_parquet(GAMES_DATA_FILE)

    expr = pl.col("genre") == genre.value
    if title:
        expr &= pl.col("title").str.contains(title)

    filtered_games = df.filter(expr).select(["genre", "title", "price"]).to_dicts()

    return {"games": filtered_games}


@app.get("/v2/games/{genre}")
async def games_v2(genre: GameGenre, title: str = ""):
    """
    Get a list of games filtered by genre and optional title using DuckDB.
    """
    conn = duckdb.connect(database=":memory:")

    query = f"""
        SELECT genre, title, price
        FROM read_parquet('{GAMES_DATA_FILE}')
        WHERE genre = '{genre.value}'
    """

    if title:
        query += f" AND title ILIKE '%{title}%'"

    query += " ORDER BY price DESC LIMIT 100"

    result = conn.execute(query).fetchall()
    conn.close()

    games = [{"genre": row[0], "title": row[1], "price": row[2]} for row in result]

    stats = {
        "total_nr_games": len(games),
        "total_price": round(sum(game["price"] for game in games), 2),
        "average_price": round(sum(game["price"] for game in games) / len(games), 2)
        if games
        else 0,
    }

    return {"games": games, "stats": stats}
