import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { rxResource } from '@angular/core/rxjs-interop';
import { MatCardModule } from '@angular/material/card';
import { AdminService } from '../../services/admin.service';
import { LoadingComponent } from '../../../../shared/components/loading/loading';
import { ErrorMessageComponent } from '../../../../shared/components/error-message/error-message';

@Component({
  selector: 'app-admin-dashboard',
  templateUrl: './admin-dashboard.html',
  styleUrl: './admin-dashboard.scss',
  imports: [RouterLink, MatCardModule, LoadingComponent, ErrorMessageComponent],
})
export class AdminDashboardComponent {
  private adminService = inject(AdminService);

  boards = rxResource({
    stream: () => this.adminService.getAdminBoards(),
  });
}
