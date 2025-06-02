package com.farmkeeper.mobile.ui.login

import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.farmkeeper.mobile.data.model.LoginResponse
import com.farmkeeper.mobile.data.model.UserRole
import com.farmkeeper.mobile.data.repository.ApiProvider
import com.farmkeeper.mobile.data.repository.AuthRepositoryImpl
import com.farmkeeper.mobile.data.repository.UserRepository
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch

class LoginViewModel(
    private val repository: AuthRepositoryImpl,
    private val userRepository: UserRepository
) : ViewModel() {

    private val _loginResult = MutableStateFlow<Result<LoginResponse>?>(null)
    val loginResult: StateFlow<Result<LoginResponse>?> = _loginResult

    private val _userRole = MutableStateFlow<UserRole?>(null)
    val userRole: StateFlow<UserRole?> = _userRole

    private val _userFarmId = MutableStateFlow<String?>(null)
    val userFarmId: StateFlow<String?> = _userFarmId

    fun login(email: String, password: String) {
        viewModelScope.launch {
            val result = repository.login(email, password)
            _loginResult.value = result
            if (result.isSuccess) {
                ApiProvider.authToken = result.getOrNull()?.token
                val roleInt = result.getOrNull()?.role ?: -1
                val role = UserRole.fromInt(roleInt)
                _userRole.value = role

                if (role == UserRole.Worker || role == UserRole.Admin) {
                    val farmResult = userRepository.getUserFarmId()
                    if (farmResult.isSuccess) {
                        _userFarmId.value = farmResult.getOrNull()
                    } else {
                        Log.e("Login", "Failed to load user farm id: ${farmResult.exceptionOrNull()}")
                    }
                }
            }
        }
    }
}


