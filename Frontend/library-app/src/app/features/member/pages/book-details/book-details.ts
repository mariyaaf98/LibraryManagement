import { ChangeDetectorRef, Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BookService } from '../../../../core/services/book';

@Component({
  selector: 'app-book-details',
  imports: [],
  templateUrl: './book-details.html',
  styleUrl: './book-details.css',
})
export class BookDetailsComponent {

  book: any;
  activeTab: string = 'description';

  constructor(
    private route: ActivatedRoute,
    private bookService: BookService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id')!;

    this.bookService.getBookById(id).subscribe(res => {
      this.book = res;
      this.cdr.detectChanges();
    });
  }
  setTab(tab: string) {
    this.activeTab = tab;
  }
}
