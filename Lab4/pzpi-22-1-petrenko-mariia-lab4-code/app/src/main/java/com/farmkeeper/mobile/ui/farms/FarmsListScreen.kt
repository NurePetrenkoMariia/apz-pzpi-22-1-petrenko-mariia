package com.farmkeeper.mobile.ui.farms

import androidx.compose.foundation.layout.*
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material3.*
import androidx.compose.runtime.*
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import com.farmkeeper.mobile.data.model.FarmDto
import androidx.compose.ui.res.stringResource
import com.farmkeeper.mobile.R
import com.farmkeeper.mobile.data.model.UserRole


@Composable
fun FarmsListScreen(
    viewModel: FarmViewModel,
    userRole: UserRole,
    onNavigateToAssignments: (String) -> Unit,
    onNavigateToAdminPanel: () -> Unit
){
    val farms by viewModel.farms.collectAsState()
    val error by viewModel.error.collectAsState()

    LaunchedEffect(Unit) {
        viewModel.loadFarms()
    }

    Column(modifier = Modifier.fillMaxSize()
        .padding(top = 32.dp, start = 16.dp, end = 16.dp)) {
        Text(
            text = stringResource(R.string.your_farms),
            style = MaterialTheme.typography.headlineMedium
        )
        Spacer(modifier = Modifier.height(8.dp))

        if (error != null) {
            Text(
                text = "${stringResource(R.string.error_prefix)} $error",
                color = MaterialTheme.colorScheme.error
            )
        }
        if (userRole == UserRole.DatabaseAdmin) {
            Button(onClick = onNavigateToAdminPanel ) {
                Text(text = stringResource(R.string.admin_panel))
            }
        }
        LazyColumn {
            items(farms) { farm ->
                FarmItem(farm = farm, onTasksClick = onNavigateToAssignments)
                HorizontalDivider()
            }
        }
    }
}

@Composable
fun FarmItem(farm: FarmDto,onTasksClick: (String) -> Unit) {
    Column(modifier = Modifier.padding(vertical = 8.dp)) {
        Text(text = "${stringResource(R.string.label_name)} ${farm.name}", style = MaterialTheme.typography.titleMedium)
        Text(text = "${stringResource(R.string.label_country)} ${farm.country}", style = MaterialTheme.typography.bodyMedium)
        Text(text = "${stringResource(R.string.label_city)} ${farm.city}", style = MaterialTheme.typography.bodyMedium)
        Text(text = "${stringResource(R.string.label_street)} ${farm.street}", style = MaterialTheme.typography.bodyMedium)


        Spacer(modifier = Modifier.height(8.dp))

        Button(onClick = { onTasksClick(farm.id) }) {
            Text(text = stringResource(R.string.go_to_tasks))
        }
    }
}