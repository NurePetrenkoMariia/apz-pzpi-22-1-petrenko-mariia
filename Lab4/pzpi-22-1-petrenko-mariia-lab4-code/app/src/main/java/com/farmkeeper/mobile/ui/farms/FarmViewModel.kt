package com.farmkeeper.mobile.ui.farms

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import com.farmkeeper.mobile.data.model.FarmDto
import com.farmkeeper.mobile.data.repository.FarmRepository
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch

class FarmViewModel(private val farmRepository: FarmRepository) : ViewModel(){
    private val _farms = MutableStateFlow<List<FarmDto>>(emptyList())
    val farms: StateFlow<List<FarmDto>> = _farms

    private val _error = MutableStateFlow<String?>(null)
    val error: StateFlow<String?> = _error
    fun loadFarms() {
        viewModelScope.launch {
            val result = farmRepository.getFarms()
            if (result.isSuccess) {
                _farms.value = result.getOrDefault(emptyList())
            } else {
                _error.value = result.exceptionOrNull()?.message ?: "Error"
            }
        }
    }
}