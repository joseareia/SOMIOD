﻿using Newtonsoft.Json.Linq;
using SOMIOD.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Http;

namespace SOMIOD.Controllers
{
    public class ModuleController : DatabaseConnection
    {

        private List<Module> modules;

        public ModuleController() { 
            this.modules = new List<Module>();
        }

        public List<Module> GetModules() {

            List<Module> modules = new List<Module>();
            setSqlComand("SELECT * FROM Module ORDER BY Id");
            try
            {
                connect();
                Select();
                disconnect();

            }
            catch (Exception e) 
            {
                return null;
            }
            return new List<Module>(this.modules);
        }

        public Module GetModule(int id)
        {
            try
            {
                connect();
                setSqlComand("SELECT * FROM modules WHERE Id=@id");
                Select(id);
                disconnect();

                if (this.modules[0] == null)
                {
                    return null;
                }
                return this.modules[0];

            }
            catch (Exception)
            {
                //fechar ligação à DB
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    disconnect();
                }
                return null;
                //return BadRequest();
            }
        }

        public int PostModule(Module module) {
            try
            {
                connect();
                // id, string mname, DateTime creation_dt, int parent

                string sql = "INSERT INTO Prods VALUES (@Id,@Name,@Creation_date,@Parent)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", module.Id);
                cmd.Parameters.AddWithValue("@Name", module.Name);
                cmd.Parameters.AddWithValue("@Creation_dt", module.Creation_dt);
                cmd.Parameters.AddWithValue("@Parent", module.Parent);
                setSqlComand(sql);

                int numRow = InsertOrUpdate(cmd);
                disconnect();
                if (numRow == 1)
                {
                    return numRow;
                }
                return -1;

            }
            catch (Exception)
            {
                //fechar ligação à DB
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    disconnect();
                }
                return -1;
            }
        }

        public int UpdateModule(Module module)
        {
            try
            {
                connect();
                string sql = "UPDATE Prods SET Id = @Id, Name = @Name, Creation_date = @Creation_date, Parent = @Parent WHERE id = @id";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Id", module.Id);
                cmd.Parameters.AddWithValue("@Name", module.Name);
                cmd.Parameters.AddWithValue("@Creation_dt", module.Creation_dt);
                cmd.Parameters.AddWithValue("@Parent", module.Parent);
                setSqlComand(sql);

                int numRow = InsertOrUpdate(cmd);

                disconnect();
                if (numRow == 1)
                {
                    return numRow;
                }
                return -1;

            }
            catch (Exception)
            {
                //fechar ligação à DB
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    disconnect();
                }

                return -1;
            }
        }

        public int DeleteModule(int id) {
            try
            {
                connect();

                setSqlComand("DELETE FROM Prods WHERE id = @id");
                int numRow = Delete(id);
                disconnect();
                if (numRow == 1)
                {
                    return 1;
                }
                return -1;

            }
            catch (Exception)
            {
                //fechar ligação à DB
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    disconnect();
                }
                return -1;
            }
        }

        public override void readerIterator(SqlDataReader reader) {
            this.modules = null;
            this.modules = new List<Module>();
            while (reader.Read())
            {
                Module module = new Module((int)reader["id"], (string)reader["name"], new DateTime(), (int)reader["parent"]);

                this.modules.Add(module);
            }
        }
    }
}