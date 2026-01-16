#!/bin/bash
set -e

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
MOBILE_DIR="$(dirname "$SCRIPT_DIR")"
ROOT_DIR="$(dirname "$MOBILE_DIR")"
CONTRACTS_DIR="$ROOT_DIR/contracts"
API_PROJECT="$ROOT_DIR/src/HealthScan.Api"
API_URL="http://localhost:5000"

mkdir -p "$CONTRACTS_DIR"

echo "Starting backend server..."
cd "$API_PROJECT"
dotnet run --urls "$API_URL" &
SERVER_PID=$!

cleanup() {
    echo "Stopping backend server..."
    kill $SERVER_PID 2>/dev/null || true
}
trap cleanup EXIT

echo "Waiting for server to start..."
for i in {1..30}; do
    if curl -s "$API_URL/health" > /dev/null 2>&1; then
        echo "Server is ready!"
        break
    fi
    sleep 1
done

echo "Fetching OpenAPI spec..."
curl -s "$API_URL/swagger/v1/swagger.json" > "$CONTRACTS_DIR/openapi.json"

if [ ! -s "$CONTRACTS_DIR/openapi.json" ]; then
    echo "Error: Failed to fetch OpenAPI spec"
    exit 1
fi

echo "Generating TypeScript client..."
cd "$MOBILE_DIR"
npx openapi-typescript-codegen \
    --input "$CONTRACTS_DIR/openapi.json" \
    --output ./src/infrastructure/api/generated \
    --client axios \
    --useOptions \
    --useUnionTypes

echo "API client generated successfully!"
