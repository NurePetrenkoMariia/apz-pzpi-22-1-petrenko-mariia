package com.farmkeeper.mobile.ui.admin_panel

import android.net.Uri
import android.util.Log
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.foundation.layout.*
import androidx.compose.material3.Button
import androidx.compose.material3.Text
import androidx.compose.runtime.*
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.res.stringResource
import androidx.compose.ui.unit.dp
import com.farmkeeper.mobile.R
import com.farmkeeper.mobile.ui.admin_panel.AdminViewModel
import java.io.File

@Composable
fun AdminPanelScreen(viewModel: AdminViewModel) {
    val context = LocalContext.current

    Column(
        modifier = Modifier
            .fillMaxSize()
            .padding(16.dp),
        verticalArrangement = Arrangement.Center,
        horizontalAlignment = Alignment.CenterHorizontally
    ) {
        Button(onClick = {
            Log.d("AdminPanel", "UI: Клік по кнопці")
            viewModel.createBackup()
        }) {

            Text(text = stringResource(R.string.create_backup))
        }

        Spacer(modifier = Modifier.height(16.dp))

        Button(onClick = {
            val file = File(
                context.getExternalFilesDir(null),
                "DatabaseBackup.bak"
            )
            if (file.exists()) {
                viewModel.restoreBackup(file)
            } else {

                Log.e("Backup", "Файл не знайдено")
            }
        }) {
            Text(text = stringResource(R.string.restore_backup))
        }

    }
}
