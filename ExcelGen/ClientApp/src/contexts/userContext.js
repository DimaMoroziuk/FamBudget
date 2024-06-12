import { createContext, useState, useEffect, useCallback } from 'react';

export function useUser(){
    const [user, setUser] = useState({});
  
    useEffect(() => {
        fetch(`api/Account/GetUser`).then(response => 
          {
            if(response.status !== 400)
              return response.json();
            else
              return {};
          })
          .then(data => {
            setUser(data);
          });
    }, []);

    const setUserData = useCallback((newUserData) => {
        setUser(newUserData);
    })

    return { user, setUser, setUserData }
}

export const userContext = createContext(null);

// export userContext;