import React from "react";
import './CustomButton.css';

const CustomButton = ({
  // Nội dung
  image,
  icon, // React icon component
  label,
  description,
  
  // Styling
  variant = "primary", // primary, secondary, success, warning, danger, info
  size = 140, // small, medium, large, xlarge hoặc số (px)
  shape = "rounded", // rounded, circle, square
  
  // Colors
  bgColor = "#27ae60",
  textColor,
  hoverColor,
  
  // Layout
  direction = "vertical", // vertical, horizontal
  fullWidth = false,
  
  // Behavior
  disabled = false,
  loading = false,
  onClick,
  
  // Animation
  animation = "scale", // scale, bounce, pulse, none
  
  // Custom styles
  customStyle = {},
  className = "",
  
  // Legacy props for backward compatibility
  iconSize = 70, // Kích thước icon/ảnh (px)
  
  // Accessibility
  ariaLabel,
  title,
}) => {
  // Xử lý click event
  const handleClick = (e) => {
    if (disabled || loading) return;
    if (onClick) onClick(e);
  };

  // Xử lý keyboard events
  const handleKeyDown = (e) => {
    if (e.key === 'Enter' || e.key === ' ') {
      e.preventDefault();
      handleClick(e);
    }
  };

  // Tạo class names
  const buttonClasses = [
    'custom-button',
    `custom-button--${variant}`,
    `custom-button--${size}`,
    `custom-button--${shape}`,
    `custom-button--${direction}`,
    animation !== 'none' && `custom-button--${animation}`,
    fullWidth && 'custom-button--full-width',
    disabled && 'custom-button--disabled',
    loading && 'custom-button--loading',
    className
  ].filter(Boolean).join(' ');

  // Xử lý size (hỗ trợ cả string và number)
  const getButtonSize = () => {
    if (typeof size === 'number') {
      return {
        width: `${size}px`,
        height: `${size}px`,
        minHeight: `${size}px`,
      };
    }
    return {};
  };

  // Xử lý iconSize
  const getIconSize = () => {
    if (iconSize) {
      return {
        width: `${iconSize}px`,
        height: `${iconSize}px`,
      };
    }
    return {};
  };

  // Inline styles
  const inlineStyles = {
    ...customStyle,
    ...getButtonSize(),
    ...(bgColor && { '--button-bg-color': bgColor }),
    ...(textColor && { '--button-text-color': textColor }),
    ...(hoverColor && { '--button-hover-color': hoverColor }),
  };

  return (
    <div
      className={buttonClasses}
      style={inlineStyles}
      onClick={handleClick}
      onKeyDown={handleKeyDown}
      tabIndex={disabled ? -1 : 0}
      role="button"
      aria-label={ariaLabel || label}
      title={title || label}
    >
      <div className="custom-button__content">
        {/* Icon/Image container */}
        <div className="custom-button__icon-container">
          {loading ? (
            <div className="custom-button__spinner">
              <div className="spinner"></div>
            </div>
          ) : (
            <>
              {icon && <div className="custom-button__icon">{icon}</div>}
              {image && (
                <img
                  src={image}
                  alt={label}
                  className="custom-button__image"
                  style={getIconSize()}
                />
              )}
            </>
          )}
        </div>

        {/* Text content */}
        {(label || description) && (
          <div className="custom-button__text">
            {label && (
              <span className="custom-button__label">{label}</span>
            )}
            {description && (
              <span className="custom-button__description">{description}</span>
            )}
          </div>
        )}
      </div>

      {/* Ripple effect */}
      <div className="custom-button__ripple"></div>
    </div>
  );
};

export default CustomButton;
