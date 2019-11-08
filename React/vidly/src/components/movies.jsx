import React, { Component } from "react";
import { getMovies } from "../services/fakeMovieService";
import { Like } from "./common/like";
import Pagination from "./common/pagination";
import paginate from "./../utilities/paginate";
import { ListGroup } from "./listGroup";
import { getGenres } from "./../services/fakeGenreService";

export class Movies extends Component {
  state = {
    movies: [],
    genre: [],
    renderMoviesPerPage: 4,
    activePage: 1,
    selectedGenre: {}
  };

  componentDidMount() {
    const movies = getMovies();
    const genre = getGenres();
    this.setState({ movies, genre, selectedGenre: genre[0] });
  }

  handrelDelete = movie => {
    const movies = this.state.movies.filter(m => m._id !== movie._id);
    this.setState({ movies });
  };

  handleLike = movie => {
    const movies = [...this.state.movies];
    const index = movies.indexOf(movie);
    movies[index] = { ...movie };
    movies[index].liked = !movies[index].liked;
    this.setState({ movies });
    //ToDo: code for updating the database
  };

  handlePageSelect = selectedPage => {
    this.setState({ activePage: selectedPage });
  };

  handleGenreSelect = genre => {
    this.setState({ selectedGenre: genre });
  };

  render() {
    const { length: count } = this.state.movies;
    const {
      activePage,
      renderMoviesPerPage,
      movies: allMovies,
      genre,
      selectedGenre
    } = this.state;

    const movies = paginate(allMovies, activePage, renderMoviesPerPage);

    if (count === 0) return <p>There are no movies in the database</p>;

    return (
      <div className="row">
        <div className="col-3">
          <ListGroup
            item={genre}
            onItemSelect={this.handleGenreSelect}
            selectedItem={selectedGenre}
          />
        </div>
        <div className="col-9">
          <p> There are total of {count} movies in the database </p>
          <table className="table">
            <thead>
              <tr>
                <th>Title</th>
                <th>Genre</th>
                <th>Stock</th>
                <th>Rate</th>
                <th>Like</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {movies.map(movie => (
                <tr key={movie._id}>
                  <td>{movie.title}</td>
                  <td>{movie.genre.name}</td>
                  <td>{movie.numberInStock}</td>
                  <td>{movie.dailyRentalRate}</td>
                  <td>
                    <Like
                      isLiked={movie.liked}
                      onLike={() => this.handleLike(movie)}
                    />
                  </td>
                  <td>
                    <button
                      onClick={() => this.handrelDelete(movie)}
                      className="btn btn-danger btn-sm"
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
          <Pagination
            moviesNumber={count}
            moviesPerPage={renderMoviesPerPage}
            onPageSelect={this.handlePageSelect}
            activePage={activePage}
          />
        </div>
      </div>
    );
  }
}
