using System;
using Factions;
using Nodes.Abstractions;
using Pawns;

namespace Abstractions
{
    public interface IPawnStateService
    {
       // public IPawn CreatePawn(Faction faction, INode baseNode);

        public void EnterPawnToGame(IPawn pawn, Action onPawnEntered);

        public void ReturnPawnToBase(IPawn pawn);
    }
}