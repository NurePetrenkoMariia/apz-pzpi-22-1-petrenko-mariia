package com.farmkeeper.mobile.data.model

enum class UserRole(val value: Int) {
    Owner(0),
    Admin(1),
    Worker(2),
    DatabaseAdmin(3);

    companion object {
        fun fromInt(value: Int): UserRole? = values().find { it.value == value }
    }
}