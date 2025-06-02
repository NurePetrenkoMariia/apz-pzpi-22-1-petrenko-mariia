package com.farmkeeper.mobile.ui.admin_panel

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.farmkeeper.mobile.data.repository.AdminRepository
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.launch
import java.io.File
import android.util.Log

class AdminViewModel(private val repository: AdminRepository): ViewModel(){
    private val _backupState = MutableStateFlow<String?>(null)
    val backupState: StateFlow<String?> = _backupState

    private val _restoreState = MutableStateFlow<String?>(null)
    val restoreState: StateFlow<String?> = _restoreState

    fun createBackup() {
        viewModelScope.launch {
            Log.d("AdminPanel", "UI: Старт створення бекапу")

            val result = repository.createBackup()

            result.onSuccess { message ->
                Log.d("AdminPanel", "VM: Успіх — $message")
                _backupState.value = message
            }.onFailure { error ->
                Log.e("AdminPanel", "VM: Помилка — ${error.message}", error)
                _backupState.value = error.message
            }
        }
    }

    fun restoreBackup(file: File) {
        viewModelScope.launch {
            try {
                Log.d("AdminPanel", "UI: Старт відновлення файлу: ${file.absolutePath}")
                val result = repository.restoreBackup(file)

                if (result.isSuccess) {
                    Log.d("AdminPanel", "UI: Відновлення успішне: ${result.getOrNull()}")
                    _restoreState.value = result.getOrNull()
                } else {
                    val errorMsg = result.exceptionOrNull()?.message ?: "Невідома помилка"
                    Log.e("AdminPanel", "UI: Відновлення не вдалося: $errorMsg")
                    _restoreState.value = errorMsg
                }
            } catch (e: Exception) {
                Log.e("AdminPanel", "UI: Виняток під час відновлення", e)
                _restoreState.value = "Виняток: ${e.message}"
            }
        }
    }

}