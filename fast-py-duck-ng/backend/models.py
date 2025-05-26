from enum import Enum
from dataclasses import dataclass


class BicycleBrand(str, Enum):
    Canyon = "Canyon"
    Trek = "Trek"
    Specialized = "Specialized"
    Giant = "Giant"
    Cannondale = "Cannondale"


class GameGenre(str, Enum):
    RolePlaying = "Role-Playing"
    Strategy = "Strategy"
    Shooter = "Shooter"


@dataclass
class Game:
    genre: GameGenre
    title: str
    price: float
