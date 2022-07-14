import React, { useState, useEffect } from "react";
import "./OnlineSystemHeader.css"

const OnlineSystemHeader = () => {
    const [smaller, setSmaller] = useState(false);

    useEffect(() => {
        const handleScroll = resizeHeaderOnScroll;
        window.addEventListener("scroll", handleScroll, { passive: true });
        return () => window.removeEventListener("scroll", handleScroll);
    }, []);

    const resizeHeaderOnScroll = () => {
        const distanceY = (window.pageYOffset || document.documentElement.scrollTop);
        const shrinkOn = 20;

        if (distanceY > shrinkOn) {
            setSmaller(true);
        } else {
            setSmaller(false);
        }
    }

    return (
        <div id="wrapper">
            <header id="js-header" className={smaller? "smaller": ""}>
                <img src={require('./logo.png')} />
                <div className="container clearfix">
                    <h1 id="logo">Online Learning System</h1>
                </div>
            </header>
        </div>
    );
    
}
export default OnlineSystemHeader;