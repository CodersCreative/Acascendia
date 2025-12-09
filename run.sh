surreal start memory --allow-all --unauthenticated &
sleep 3 && dotnet run --project Server &
wait -n
