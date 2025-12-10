#!/usr/bin/env bash
set -euo pipefail

HOST=127.0.0.1
PORT=8000
surreal_started=0
surreal_pid=""

port_open() {
	(echo > /dev/tcp/${HOST}/${PORT}) >/dev/null 2>&1
}

if port_open; then
	echo "SurrealDB already running at ${HOST}:${PORT}"
else
	echo "Starting SurrealDB..."
	surreal start --allow-all --unauthenticated rocksdb://$HOME/.config/studbud/database.db &
	surreal_pid=$!
	surreal_started=1

	sleep 3
	if port_open; then
		echo "SurrealDB started (pid ${surreal_pid})"
	else
		echo "Warning: SurrealDB did not appear to start on ${HOST}:${PORT}" >&2
	fi
fi

cleanup() {
	if [ "${surreal_started}" -eq 1 ] && [ -n "${surreal_pid}" ]; then
		echo "Stopping SurrealDB (pid ${surreal_pid})..."
		kill "${surreal_pid}" 2>/dev/null || true
		sleep 1
		if port_open; then
			echo "SurrealDB still listening, forcing kill..."
			kill -9 "${surreal_pid}" 2>/dev/null || true
		fi
	fi
}
trap cleanup INT TERM EXIT

dotnet run --project Server &
dotnet_pid=$!

wait ${dotnet_pid}

exit $?
