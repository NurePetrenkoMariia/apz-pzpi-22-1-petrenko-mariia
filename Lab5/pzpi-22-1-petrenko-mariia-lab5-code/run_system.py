import subprocess
import os
import urllib.request
import time
import sys
import shutil

BASE_DIR = os.path.dirname(os.path.abspath(__file__))
BACKEND_PATH = os.path.join(BASE_DIR, "FarmKeeper")
FRONTEND_PATH = os.path.join(BASE_DIR, "client")

DOTNET_DOWNLOAD_PAGE = "https://dotnet.microsoft.com/en-us/download/dotnet/8.0"
NODEJS_URL = "https://nodejs.org/dist/v20.11.1/node-v20.11.1-x64.msi"

def ask_and_install_node(name, url, filename):
    answer = input(f"{name} не встановлено. Завантажити і встановити? (y/n): ").strip().lower()
    if answer == 'y':
        print(f"Завантаження {name}...")
        urllib.request.urlretrieve(url, filename)
        print(f"Запуск інсталятора {filename}...")
        subprocess.run(filename, shell=True)
        print(f"Після встановлення потрібно перезапустити скрипт.")
        sys.exit(0)
    else:
        print(f"Без {name} робота системи не можлива.")
        sys.exit(1)

def check_dotnet_installed():
    if shutil.which("dotnet") is None:
        print(f".NET SDK не встановлено.")
        print(f"Будь ласка, встановіть його з офіційного сайту:\n{DOTNET_DOWNLOAD_PAGE}")
        sys.exit(1)
    else:
        print(".NET SDK знайдено.")

def check_node_installed():
    if shutil.which("node") is None:
        ask_and_install_node("Node.js", NODEJS_URL, "node-installer.msi")
    else:
        print("Node.js знайдено.")

def run_command(command, cwd=None):
    return subprocess.Popen(command, cwd=cwd, shell=True)

def wait_for_backend(url, timeout=60, interval=2):
    print(f"Очікуємо запуск бекенду за {url} ...")
    start_time = time.time()
    while True:
        try:
            with urllib.request.urlopen(url) as response:
                if response.status == 200:
                    print("Бекенд запущено і відповідає!")
                    break
        except Exception:
            pass

        if time.time() - start_time > timeout:
            print("Час очікування бекенду вичерпано.")
            sys.exit(1)

        print("Бекенд ще не готовий, чекаємо...")
        time.sleep(interval)

def main():
    print("Початок налаштування...")
    print("Перевірка встановлених залежностей:")
    check_dotnet_installed()
    check_node_installed()
    print()

    print("Запуск серверної частини...")
    backend_process = run_command("dotnet run --project FarmKeeper/FarmKeeper.csproj", cwd=BACKEND_PATH)

    wait_for_backend("http://localhost:5266/swagger")

    print("Встановлення залежностей фронтенду...")
    subprocess.run("npm install", cwd=FRONTEND_PATH, shell=True)

    print("Запуск фронтенду...")
    frontend_process = run_command("npm run dev", cwd=FRONTEND_PATH)

    print("\nСистема розгорнута успішно!")
    print("Серверна частина доступна за адресою:")
    print("   http://localhost:5266/swagger")
    print("Клієнтська частина доступна за адресою:")
    print("   http://localhost:5173")

if __name__ == "__main__":
    main()
