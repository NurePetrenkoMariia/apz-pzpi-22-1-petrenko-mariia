# -*- coding: utf-8 -*-

import subprocess
import getpass
import os
import json

#Функція для встановлення Chocolatey
def install_chocolatey():
    print("Starting setup of the Chocolatey package manager.")
    installation_string = (
         "powershell -NoProfile -InputFormat None -ExecutionPolicy Bypass -Command "
         "iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'));"
    )

    try:
        subprocess.run(installation_string, shell=True, check=True)
        print("Chocolatey is installed.")
    except subprocess.CalledProcessError as e:
        print("Error installing the Chocolatey package manager.")

#Функція для встановлення .NET 8.0
def install_dotnet():
    print("You don't have .NET installed.")
    print("Starting installation.")
    try:
       subprocess.run(["choco", "install", "dotnet-8.0-sdk", "-y"], check=True)
       print(".NET 8 is installed.")
    except subprocess.CalledProcessError as e:
       print("Error installing .NET 8.")

#Функція для встановлення SQL Server
def install_sql_server():
    print("You don't have MS SQL Server installed.")
    print("Starting installation.")
    try:
        subprocess.run(["choco", "install", "sql-server", "-y"], check=True)
        print("SQL Server is installed.")
    except subprocess.CalledProcessError as e:
        print("Error installing SQL Server")

#Функція для налаштування бази даних
def db_setup(server, username, password, db_name):
    print("Looking for backup file")
    backup_path = os.path.abspath(os.path.join("Migrations", "FarmKeeperDatabaseBackup.bak"))
    if not os.path.isfile(backup_path):
        print(f"Backup file '{backup_path}' not found.")
        return
    else:
        print("Backup file found successfully.")

    print("Looking for database")

    check_db_command = f"""
    sqlcmd -S {server} -U {username} -P {password} -Q "SELECT name FROM sys.databases WHERE name = '{db_name}'"
    """
    check_result = subprocess.run(check_db_command, shell=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE, text=True)

    if db_name in check_result.stdout:
        print(f"'{db_name}' database already exists.")
    else:
        print(f"'{db_name}' database was not found. Creating database.")
        create_db_command = f"""
            sqlcmd -S {server} -U {username} -P {password} -Q "CREATE DATABASE {db_name}"
            """
        create_result = subprocess.run(create_db_command, shell=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE, text=True)
        if create_result.returncode == 0:
            print(f"'{db_name}' database created successfully.")
        else:
            print(f"Error creating database: {create_result.stderr.strip()}")
            return
        
        print("Restoring database from the backup file")
        restore_db_command = ( "sqlcmd -S " + server + " -U " + username + " -P " + password + " "
             "-Q \"ALTER DATABASE " + db_name + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE; "
             "RESTORE DATABASE " + db_name + " FROM DISK = '" + backup_path + "' "
             "WITH MOVE 'FarmKeeperDb' TO 'D:\\Program Files\\Microsoft SQL Server\\MSSQL16.MSSQLSERVER\\MSSQL\\DATA\\" + db_name + ".mdf', "
             "MOVE 'FarmKeeperDb_log' TO 'D:\\Program Files\\Microsoft SQL Server\\MSSQL16.MSSQLSERVER\\MSSQL\\DATA\\" + db_name + "_log.ldf', "
             "REPLACE\"" )
        restore_result = subprocess.run(restore_db_command, shell=True, stdout=subprocess.PIPE, stderr=subprocess.PIPE, text=True)
        if restore_result.returncode == 0:
            print("Data base successfully restored from the backup file.")
        else:
            print("Error restoring the data base from the backup file.")

#Функція для зміни рядка підключення
def change_connect_string(server, username, password, db_name):
    current_directory = os.getcwd()
    appsettings_path = os.path.join(current_directory, 'appsettings.json')
    if os.path.exists(appsettings_path):
        print(f"File appsettings.json was found.")
        try:
            with open(appsettings_path, "r") as file:
                data = json.load(file)

            connection_string = (
                f"Server={server};Database={db_name};User Id={username};Password={password};"
                "Encrypt=True;TrustServerCertificate=True"
            )
            data['ConnectionStrings']['FarmKeeperConnectionString'] = connection_string

            with open(appsettings_path, "w") as file:
                json.dump(data, file, indent=4)
            print("Connection string updated successfully.")
        except Exception as e:
            print(f"Error updating connection string: {e}")
    else:
        print(f"File appsettings.json was not found.")

def main():
    print("Starting setup of the program")
    #Перевірка чи встановлений менеджер пакетів Chocolatey на пристрої
    try:
        subprocess.run(["choco", "-v"], stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL, check=True)
        print("The Chocolatey package manager was found.")
    except FileNotFoundError:
        install_chocolatey()

    #Перевірка чи встановлений .NET
    try:
        subprocess.run(["dotnet", "--version"], stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL, check=True)
        print(".NET was found.")
    except FileNotFoundError:
        install_dotnet()

    #Перевірка чи встановлений MS SQL Server
    try:
        subprocess.run(["sqlcmd", "-?"], stdout=subprocess.DEVNULL, stderr=subprocess.DEVNULL, check=True)
        print("SQL Server was found.")
    except FileNotFoundError:
        install_sql_server()

    server = "Mariia-PC79"
    username = "sa"
    password = getpass.getpass("Enter password: ")
    db_name = "FarmKeeperDb"

    #Налаштування бази даних
    db_setup(server, username, password, db_name)

    #Налаштування appsettings.json
    change_connect_string(server, username, password, db_name)

if __name__ == "__main__":
    main()