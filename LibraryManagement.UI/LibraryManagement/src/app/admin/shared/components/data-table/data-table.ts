import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-data-table',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './data-table.html',
  styleUrls: ['./data-table.css']
})
export class DataTableComponent {

  @Input() columns: string[] = [];
  @Input() data: any[] = [];

}