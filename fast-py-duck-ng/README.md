# Fast Py Duck Ng

## Setup backend

curl -LsSf https://astral.sh/uv/install.sh | sh
uv init backend
uv add ruff duckdb faker polars pydantic-settings
uv add fastapi --extra standard
uv add pyright --dev

uv run fastapi dev
uv run pyright

## Setup frontend

npm install -g @angular/cli
