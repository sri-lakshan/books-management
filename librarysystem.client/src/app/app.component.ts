import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

interface Book {
  id: number;
  title: string;
  yearOfRelease: number;
  author: string;
}

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  public books: Book[] = [];
  public borrowedBooks: Book[] = [];
  public searchResults: string = "";

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getBooks();
  }

  getBooks() {
    this.http.get<Book[]>('/weatherforecast').subscribe(
      (result) => {
        this.books = result;
      },
      (error) => {
        console.error(error);
      }
    );
  }

  borrowBook(bookId: number) {
    this.http.post(`/weatherforecast/borrow/${bookId}`, {}).subscribe(
      (response: any) => {
        if (response.success) {
          const borrowedBook = this.books.find(book => book.id === bookId);
          this.books = this.books.filter(book => book.id !== bookId);

          if (borrowedBook) {
            // Add the book to borrowedBooks array
            this.borrowedBooks.push(borrowedBook);
          }
        } else {
          alert(response.message);
        }
      },
      (error) => {
        console.error(error);
      }
    );
  }

  validateSearchQuery(query: string): void {
    try {
      
      const illegalCharacters = /[!@#\$%\^\&*\)\(+=._-]+/g;
      if (illegalCharacters.test(query)) {
        throw new Error('Your search contains illegal characters!');
      }

      
      if (query.trim() === '') {
        throw new Error('Search query cannot be empty!');
      }

      if (query.length < 3) {
        throw new Error('Search query is too short! Please enter at least 3 characters.');
      }

      
      if (!isNaN(Number(query))) {
        throw new Error('Search query cannot be a number! Please enter a valid book title.');
      }
    } catch (error) {
      alert(error);
      throw error;
    }
  }

  runValidationTests(query: string): boolean {
    try {
      this.validateSearchQuery(query);
      return true; 
    } catch (error) {
      return false; 
    }
  }



  searchBooks(bookTitle: string) {

    if (!this.runValidationTests(bookTitle)) {
      return; 
    }

    this.searchResults = "Not Found!";

    for (let book of this.books) {
      if (book.title == bookTitle) {
        this.searchResults = `Title : ${book.title} Author : ${book.author} Released in : ${book.yearOfRelease}`;
        break;
      }
    }
  }

  title = 'librarysystem.client';
}
