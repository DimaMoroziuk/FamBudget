import React, { useState, useEffect } from 'react';
import { useHistory, useParams  } from "react-router-dom";
import Avatar from '@mui/material/Avatar';
import Button from '@mui/material/Button';
import CssBaseline from '@mui/material/CssBaseline';
import TextField from '@mui/material/TextField';
import CircularProgress from '@mui/material/CircularProgress';
import FormControlLabel from '@mui/material/FormControlLabel';
import Checkbox from '@mui/material/Checkbox';
import Link from '@mui/material/Link';
import Grid from '@mui/material/Grid';
import Box from '@mui/material/Box';
import Typography from '@mui/material/Typography';
import Container from '@mui/material/Container';;
import Select from '@mui/material/Select';
import MenuItem from '@mui/material/MenuItem';
import InputLabel from '@mui/material/InputLabel';
import { createTheme, ThemeProvider } from '@mui/material/styles';

export default function IncomeAddPage() {
    const [loading, setLoading] = useState(false);
    const [categories, setCategories] = useState([]);
    const [initialIncome, setInitialIncome] = useState({});

    const history = useHistory ();

    const { id } = useParams();

    useEffect(() => {
      setLoading(true);
        fetch(`api/Category/GetCategories`).then(response => response.json())
          .then(data => {
            setCategories(data);
            setLoading(false);
          });
      }, [setCategories]);

    useEffect(() => {
      if(id && categories)
      {
        setLoading(true);
        fetch(`api/Income/GetIncome?id=${id}`).then(response => response.json())
          .then(data => {
            setInitialIncome(data);
            setLoading(false);
          });
      }
    }, [categories, setInitialIncome]);

    const changeInput = (event) => {
      setInitialIncome({ ...initialIncome, [event.target.name]: event.target.value });
    };
  

    const handleSubmit = (event) => {
      event.preventDefault();
      const data = new FormData(event.currentTarget);
      console.log({
        email: data.get('name'),
        password: data.get('price'),
      });
      setLoading(true);
      if(id){
        fetch(`api/Income/PutIncome?id=${id}`, {
          method: 'PUT',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            ...initialIncome,
            name: data.get('name'),
            price: Number(data.get('price')),
            month: Number(data.get('month')),
            year: Number(data.get('year')),
            categoryId: data.get('categoryId'),
          })
        })
        .then(response => history.push("/incomes"));
      } else {
        fetch(`api/Income/PostIncome`, {
          method: 'POST',
          headers: { 'Content-Type': 'application/json' },
          body: JSON.stringify({
            name: data.get('name'),
            price: Number(data.get('price')),
            month: Number(data.get('month')),
            year: Number(data.get('year')),
            categoryId: data.get('categoryId'),
          })
        })
        .then(response => history.push("/incomes"));
      }
    };
  
    return (
        <Container component="main" maxWidth="xs">
          <CssBaseline />
          {loading ?
      <CircularProgress /> : 
          <Box
            sx={{
              marginTop: 8,
              display: 'flex',
              flexDirection: 'column',
              alignItems: 'center',
            }}
          >
            <Typography component="h1" variant="h5">
              {id ? "Edit the income" : "Add new income" }
            </Typography>
            <Box component="form" noValidate onSubmit={handleSubmit} sx={{ mt: 3 }}>
              <Grid container spacing={2}>
                <Grid item xs={12}>
                  <TextField
                    required
                    fullWidth
                    id="name"
                    label="Name"
                    name="name"
                    value={initialIncome?.name}
                    defaultValue={initialIncome?.name ? initialIncome?.name : ""}
                    onChange={changeInput}
                  />
                </Grid>
                <Grid item xs={12}>
                  <TextField
                    required
                    fullWidth
                    name="price"
                    label="Price"
                    id="price"
                    value={initialIncome?.price}
                    defaultValue={initialIncome?.price ? initialIncome?.price : ""}
                    onChange={changeInput}
                  />
                </Grid>
                <Grid item xs={12}>
                <InputLabel id="month-select-label">Month</InputLabel>
                <Select
                labelId="month-select-label"
                id="month"
                name="month"
                label="Month"
                fullWidth
                value={initialIncome?.month || (new Date().getMonth() + 1)}
                onChange={changeInput}
                >
                <MenuItem value={1}>January</MenuItem>
                <MenuItem value={2}>February</MenuItem>
                <MenuItem value={3}>March</MenuItem>
                <MenuItem value={4}>April</MenuItem>
                <MenuItem value={5}>May</MenuItem>
                <MenuItem value={6}>June</MenuItem>
                <MenuItem value={7}>July</MenuItem>
                <MenuItem value={8}>August</MenuItem>
                <MenuItem value={9}>September</MenuItem>
                <MenuItem value={10}>October</MenuItem>
                <MenuItem value={11}>November</MenuItem>
                <MenuItem value={12}>December</MenuItem>
                </Select>
                </Grid>
                
                <Grid item xs={12}>
                <InputLabel id="category-select-label">Category</InputLabel>
                <Select
                labelId="category-select-label"
                id="categoryId"
                name="categoryId"
                label="Category"
                fullWidth
                value={initialIncome?.categoryId}
                onChange={changeInput}
                >
                {categories.map(cat => <MenuItem value={cat.id}>{cat.name}</MenuItem>)}
                </Select>
                </Grid>
                <Grid item xs={12}>
                <TextField
                    required
                    fullWidth
                    // error={this.state.error}
                    id="year"
                    name="year"
                    label="Year"
                    onChange={changeInput}
                    value={initialIncome?.year}
                    defaultValue={initialIncome?.year || new Date().getFullYear()}
                    variant="filled"
                />
                </Grid>
              </Grid>
              <Button
                type="submit"
                fullWidth
                variant="contained"
                sx={{ mt: 3, mb: 2 }}
              >
                {id ? "Edit" : "Add"}
              </Button>
            </Box>
          </Box>}
        </Container>
    );
}