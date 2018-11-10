﻿using MyGame.DAL.Entities;
using MyGame.DAL.EntityFramework;
using MyGame.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.DAL.Repositories
{
    public class GameManager : IGameManager
    {
        /// <summary>
        /// Database context.
        /// </summary>
        public ApplicationContext Database { get; private set; }

        /// <summary>
        /// Initializes a new instance of<see cref= "GameManager" />.
        /// </summary>
        /// <param name="db">Database context.</param>
        public GameManager(ApplicationContext db)
        {
            Database = db;
        }

        #region CREATE
        public async Task<bool> CreateAsync(Game item)
        {
            try
            {
                Database.Games.Add(item);
                await Database.SaveChangesAsync();
            }
            catch
            {
                return false;
            }

            return true;
        }
        #endregion

        #region DELETE
        public async Task<bool> DeleteAsync(Game item)
        {
            try
            {
                Database.Games.Remove(item);
                await Database.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion

        #region FIND_BY_ID
        public async Task<Game> FindByIdAsync(int id)
        {
            return await Database.Games.FindAsync(id);
        }
        #endregion

        #region GET_ALL_GAMES
        public IQueryable<Game> GetAllGames()
        {
            IQueryable<Game> tables  = Database.Games;
            return tables;
        }
        #endregion

        #region GET_USER_GAMES
        public IQueryable<Game> GetGamesForUser(int userId)
        {
            IQueryable<Game> games = Database.Games.Where(g => g.Opponents.Where(o => o.Id == userId).Count() > 0);

            return games;
        }
        #endregion

        #region GET_AVAILABLE_GAMES
        public IQueryable<Game> GetAvailableGames(int userId)
        {
            IQueryable<Game> games = Database.Games.Where(t => (t.Opponents.Count == 1) && (t.Opponents.Where(o => o.Id == userId).Count() == 0));

            return games;
        }
        #endregion

        #region ADD_OPPONENT
        public async Task<Game> AddOpponentToGame(int gameId, ApplicationUser user)
        {
            Game game;
            
            try
            {
                game = await Database.Games.FindAsync(gameId);
                if (game == null)
                    return null;

                if (game.Opponents.Count != 2)
                    game.Opponents.Add(user);
                else
                {
                    if (!game.Opponents.Select(o => o.Id).Contains(user.Id))
                        return null;
                }
                await Database.SaveChangesAsync();
            }
            catch
            {
                return null;
            }
            return game;
        }
        #endregion

        #region TURN_CHANGE
        public async Task<bool> TurnChange(int gameId, int userId)
        {
            var game = await Database.Games.FindAsync(gameId);
            if (game == null)
                return false;

            game.LastTurnPlayerId = userId;
            try
            {
                await Database.SaveChangesAsync();
            }
            catch
            {
                return false;
            }
            return true;
        }
        #endregion
        public void Dispose()
        {
            Database.Dispose();
        }


    }
}
