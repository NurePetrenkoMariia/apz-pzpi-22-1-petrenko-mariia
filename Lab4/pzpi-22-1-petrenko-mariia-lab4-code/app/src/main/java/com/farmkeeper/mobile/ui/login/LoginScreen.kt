package com.farmkeeper.mobile.ui.login

import android.util.Log
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Button
import androidx.compose.material3.Text
import androidx.compose.material3.TextField
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.text.input.PasswordVisualTransformation
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.navigation.NavHostController
import com.farmkeeper.mobile.data.model.UserRole
import com.farmkeeper.mobile.ui.login.LoginViewModel
import androidx.compose.ui.res.stringResource
import com.farmkeeper.mobile.R



@Composable
fun LoginScreen(viewModel: LoginViewModel, navController: NavHostController) {

    val loginResult by viewModel.loginResult.collectAsState()
    val userRole by viewModel.userRole.collectAsState()
    val userFarmId by viewModel.userFarmId.collectAsState()

    var email by remember { mutableStateOf("") }
    var password by remember { mutableStateOf("") }
    var errorMessage by remember { mutableStateOf<String?>(null) }
    LaunchedEffect(userRole, userFarmId) {
        when (userRole) {
            UserRole.Owner, UserRole.DatabaseAdmin -> {
                navController.navigate("farms")
            }
            UserRole.Admin, UserRole.Worker -> {
                userFarmId?.let { id ->
                    navController.navigate("assignments/${id}")
                }
            }
            else -> {
                Log.e("LoginScreen", "Unknown or invalid user role: $userRole")
            }
        }
    }
    Box(
        modifier = Modifier
            .fillMaxSize(),
        contentAlignment = Alignment.Center
    ){
        Column(modifier = Modifier.padding(16.dp)) {
            Text(
                text = stringResource(R.string.login),
                fontSize = 32.sp,
                modifier = Modifier.padding(bottom = 24.dp),
                textAlign = TextAlign.Center
            )
            TextField(
                value = email,
                onValueChange = { email = it },
                label = { Text(stringResource(R.string.email)) }
            )
            TextField(
                value = password,
                onValueChange = { password = it },
                label = { Text(stringResource(R.string.password)) },
                visualTransformation = PasswordVisualTransformation()
            )

            Button(
                onClick = { viewModel.login(email, password) },
                modifier = Modifier.padding(top = 16.dp)) {
                Text(stringResource(R.string.login_btn))
            }
            loginResult?.let { result ->
                if (result.isFailure) {
                    Text(
                        "${stringResource(R.string.login_failed)} ${result.exceptionOrNull()?.message}",
                        color = Color.Red
                    )
                }
            }
        }
    }

}

