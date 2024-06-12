import React, { useState, useEffect } from 'react';
import { useHistory  } from "react-router-dom";
import Link from '@mui/material/Link';
import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import Button from '@mui/material/Button';
import TableCell from '@mui/material/TableCell';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Typography from '@mui/material/Typography';
import { mapNumberToMonthValue, getFormattedDate } from '../../helper'

export default function Purchase() {
    const [data, setData] = useState([]);
    const [deletedIds, setDeletedIds] = useState([]);
    const history = useHistory ();


    function deletePurchase(row) {
        const undeletedRows = [...data.filter(x => x.id !== row.id)];
        setData(undeletedRows);
        deletedIds.push(row.id);
        setDeletedIds([...deletedIds]);
    }
    
    function handleSave() {
      fetch(`api/Purchase/DeletePurchase`, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(deletedIds)
      })
      .then(response => setDeletedIds([]));
  }

    useEffect(() => {
        
        fetch(`api/Purchase/GetPurchases`).then(response => response.json())
        .then(data => setData(data));
      },
    [setData]);

  return (
    <React.Fragment>
      <Typography component="h2" variant="h6" color="primary" gutterBottom>
      Purchases
      </Typography>
      <Table size="medium">
        <TableHead>
          <TableRow>
            <TableCell>Name</TableCell>
            <TableCell>Added at</TableCell>
            <TableCell>Price</TableCell>
            <TableCell>Month</TableCell>
            <TableCell>Year</TableCell>
            <TableCell>Category</TableCell>
            <TableCell>Author</TableCell>
            <TableCell></TableCell>
            <TableCell align="right"></TableCell>
          </TableRow>
        </TableHead>
        <TableBody>
          {data.map((row) => {
            const date = new Date(row.addedDate);

            return (
              <TableRow key={row.id}>
                <TableCell>{row.name}</TableCell>
                <TableCell>{getFormattedDate(date)}</TableCell>
                <TableCell>{`₴${row.price}`}</TableCell>
                <TableCell>{mapNumberToMonthValue[row.month]}</TableCell>
                <TableCell>{row.year}</TableCell>
                <TableCell>{row.category.name}</TableCell>
                <TableCell>{row.author.userName}</TableCell>
                <TableCell><Link color={(row.accessType !== 1 ? "black" : "primary")} underline={(row.accessType !== 1 ? "none" : "always")}
                component="button" disabled={row.accessType !== 1} onClick={() => history.push(`/purchase/add/${row.id}`)} sx={{ mt: 3 }}>
          Edit
        </Link></TableCell>
                <TableCell align="right"><Link color={(row.accessType !== 1 ? "black" : "primary")} underline={(row.accessType !== 1 ? "none" : "always")} 
                component="button" disabled={row.accessType !== 1} onClick={() => deletePurchase(row)} sx={{ mt: 3 }}>
          Delete
        </Link></TableCell>
              </TableRow>
            )
          })}
        </TableBody>
      </Table>
      <Link color="primary" href="/purchase/add" sx={{ mt: 20 }}>
        Add new purchase
      </Link>
              <Button
                onClick={handleSave}
                fullWidth
                variant="contained"
                sx={{ mt: 3, mb: 2 }}
              >
                Save
              </Button>
      
    </React.Fragment>
  );
}