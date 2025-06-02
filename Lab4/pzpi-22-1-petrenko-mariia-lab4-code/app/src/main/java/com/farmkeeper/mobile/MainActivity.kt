package com.farmkeeper.mobile

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.foundation.layout.padding
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.navigation.NavType
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import androidx.navigation.navArgument
import com.farmkeeper.mobile.data.model.UserRole
import com.farmkeeper.mobile.data.repository.AdminRepository
import com.farmkeeper.mobile.data.repository.ApiProvider
import com.farmkeeper.mobile.data.repository.AssignmentRepository
import com.farmkeeper.mobile.data.repository.AuthRepositoryImpl
import com.farmkeeper.mobile.data.repository.FarmRepository
import com.farmkeeper.mobile.data.repository.UserRepository
import com.farmkeeper.mobile.data.repository.AdminApi
import com.farmkeeper.mobile.ui.admin_panel.AdminViewModel
import com.farmkeeper.mobile.ui.assignment.AssignmentViewModel
import com.farmkeeper.mobile.ui.farms.FarmViewModel
import com.farmkeeper.mobile.ui.farms.FarmsListScreen
import com.farmkeeper.mobile.ui.login.LoginScreen
import com.farmkeeper.mobile.ui.login.LoginViewModel
import com.farmkeeper.mobile.ui.assignment.AssignmentsScreen
import com.farmkeeper.mobile.ui.assignment.EditAssignmentScreen
import com.farmkeeper.mobile.ui.admin_panel.AdminPanelScreen


class MainActivity : ComponentActivity() {
    private lateinit var loginViewModel: LoginViewModel
    private lateinit var farmViewModel: FarmViewModel
    private lateinit var assignmentViewModel: AssignmentViewModel
    private lateinit var adminViewModel: AdminViewModel

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        val authRepository = AuthRepositoryImpl(ApiProvider.authService)
        val farmRepository = FarmRepository(ApiProvider.farmService)
        val assignmentRepository = AssignmentRepository(ApiProvider.assignmentService)
        val userRepository = UserRepository(ApiProvider.userService)
        val adminRepository = AdminRepository(ApiProvider.adminApi, applicationContext)

        loginViewModel = LoginViewModel(authRepository, userRepository)
        farmViewModel = FarmViewModel(farmRepository)
        assignmentViewModel = AssignmentViewModel(assignmentRepository)
        adminViewModel = AdminViewModel(adminRepository)

        setContent {
            val navController = rememberNavController()
            val userRole by loginViewModel.userRole.collectAsState()

            NavHost(navController, startDestination = "login") {
                composable("login") {
                    LoginScreen(viewModel = loginViewModel, navController = navController)
                }
                composable("farms") {
                    userRole?.let { it1 ->
                        FarmsListScreen(
                            viewModel = farmViewModel,
                            userRole = it1,
                            onNavigateToAssignments = { farmId ->
                                navController.navigate("assignments/$farmId")
                            },
                            onNavigateToAdminPanel = {
                                navController.navigate("admin_panel")
                            }
                        )
                    }
                }

                composable("assignments/{farmId}", arguments = listOf(navArgument("farmId") { type = NavType.StringType })) {
                    val farmId = it.arguments?.getString("farmId") ?: ""
                    AssignmentsScreen(
                        viewModel = assignmentViewModel,
                        userRole = userRole ?: UserRole.Worker,
                        farmId = farmId,
                        navController = navController
                    )
                }
                composable("edit_assignment/{assignmentId}") { backStackEntry ->
                    val assignmentId = backStackEntry.arguments?.getString("assignmentId") ?: ""
                    EditAssignmentScreen(
                        assignmentId = assignmentId,
                        viewModel = assignmentViewModel,
                        navController = navController,
                        userRole = userRole
                    )
                }
                composable("admin_panel") {
                    AdminPanelScreen(viewModel = adminViewModel)
                }


            }

        }
    }
}
